using Azure;
using Azure.Data.Tables;

namespace ProductReviewApplication.Infrastructure.AzureTables.Entities
{
    public class ProductReviewEntity : ITableEntity
    {
        //product name
        public string PartitionKey { get; set; } = "";
        //review ID
        public string RowKey { get; set; } = "";
        public string? Review { get; set; }
        public string? UserId { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }
}
