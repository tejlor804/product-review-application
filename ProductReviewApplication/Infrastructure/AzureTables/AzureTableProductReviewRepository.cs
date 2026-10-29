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

        public ProductReviewEntity? GetLatestReviewOfProduct(string productName)
        {
            IEnumerable<ProductReviewEntity> reviews = tableClient.Query<ProductReviewEntity>(x => x.PartitionKey == productName);
            return reviews.Any() ? reviews.First() : null;
        }

        public List<ProductReviewEntity> GetReviewsOfProduct(string productName)
        {
            {
                var results = new List<ProductReviewEntity>();

                foreach (var entity in tableClient.Query<ProductReviewEntity>(
                    x => x.PartitionKey == productName))
                {
                    results.Add(entity);
                }

                return results;
            }
        }

        public void SaveReview(ProductReview productReview)
        {
            tableClient.AddEntity<ProductReviewEntity>(ProductReviewMapper.toEntity(productReview));
        }
    }
}
