using Azure;
using Azure.Data.Tables;

namespace ProductReviewApplication.Infrastructure.AzureTables.Entities
{
    public class UserEntity : ITableEntity
    {

        //user ID
        public required string PartitionKey { get; set; } = string.Empty;
        //product name
        public required string RowKey { get; set; } = string.Empty;
        public string? LatestReadReviewId { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
