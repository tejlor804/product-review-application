using Azure.Data.Tables;
using ProductReviewApplication.Mapping;

namespace ProductReviewApplication.Infrastructure.AzureTables.Entities
{
    using Azure;
    using Azure.Data.Tables;

    public class ProductStateEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = "latest";
        public string? LatestReviewId { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
