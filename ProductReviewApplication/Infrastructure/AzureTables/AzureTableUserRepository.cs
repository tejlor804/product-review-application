using Azure;
using Azure.Data.Tables;
using ProductReviewApplication.Infrastructure.AzureTables.Entities;
using ProductReviewApplication.Repositories;
using System.Collections.Concurrent;
using System.Xml;

namespace ProductReviewApplication.Infrastructure.AzureTables
{
    public class AzureTableUserRepository  : IUserRepository
    {
        private readonly TableClient tableClient;
        public AzureTableUserRepository(TableClientFactory factory) {
            tableClient = factory.UsersTable;
        }
        public string? GetLatestReadReviewIdOfUser(string productName, string userId)
        {
            NullableResponse<UserEntity> response = tableClient.GetEntityIfExists<UserEntity>(userId, productName);
            return response.HasValue ? response.Value.LatestReadReviewId : null;
        }

        public void SaveLatestReadReviewIdOfUser(string userId, string productName, string reviewId)
        {
            tableClient.UpsertEntity(new UserEntity
            {

                PartitionKey = userId,
                RowKey = productName,
                LatestReadReviewId = reviewId
            });
        }
    }
}
