using Azure;
using Azure.Data.Tables;
using ProductReviewApplication.Infrastructure.AzureTables.Entities;
using ProductReviewApplication.Mapping;
using ProductReviewApplication.Repositories;
using ProductReviewApplication.Services;
using System.Collections.Concurrent;
using System.Xml;

namespace ProductReviewApplication.Infrastructure.AzureTables
{
    public class AzureTableProductReviewRepository : IProductReviewRepository
    {
        private readonly TableClient tableClient;
        public AzureTableProductReviewRepository(TableClientFactory factory)
        {
            tableClient = factory.ProductReviewsTable;
        }

        public async Task SaveReviewAndUpdateLatestAsync(
    ProductReview productReview,
    ProductStateEntity currentStatus)
        {
            var reviewEntity = ProductReviewMapper.toEntity(productReview);
            var updatedStatus = new ProductStateEntity
            {
                PartitionKey = reviewEntity.PartitionKey,
                LatestReviewId = reviewEntity.RowKey
            };
            await tableClient.SubmitTransactionAsync([
                new(
            currentStatus is null
                ? TableTransactionActionType.Add
                : TableTransactionActionType.UpdateReplace,
            updatedStatus,
            currentStatus?.ETag ?? ETag.All),
                new TableTransactionAction(
                    TableTransactionActionType.Add,
                    reviewEntity)
    ]);
            ;
        }

        public ProductStateEntity? GetLatestReviewIdOfProduct(string productName)
        {
            IEnumerable<ProductStateEntity> reviews = tableClient.Query<ProductStateEntity>(x => x.PartitionKey == productName && x.RowKey == "latest");
            return reviews.Any() ? reviews.First() : null;
        }

        public async Task<List<ProductReviewEntity>> GetReviewsOfProduct(string productName, int limit)
        {
            var query = tableClient.QueryAsync<ProductReviewEntity>(
        x => x.PartitionKey == productName
          && x.RowKey != ProductStateEntity.LatestRowKey,
        maxPerPage: limit);

            await foreach (var page in query.AsPages())
            {
                return [.. page.Values];
            }

            return [];
        }

        public void SaveReview(ProductReview productReview)
        {
            tableClient.AddEntity<ProductReviewEntity>(ProductReviewMapper.toEntity(productReview));
        }
    }
}
