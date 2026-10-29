using Azure.Data.Tables;
using ProductReviewApplication.Mapping;

namespace ProductReviewApplication.Infrastructure.AzureTables.Entities
{
    using Azure;
    using Azure.Data.Tables;

    public class ProductStateEntity : ITableEntity
    {

        public const string LatestRowKey = "latest";
        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = LatestRowKey;
        public string? LatestReviewId { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
