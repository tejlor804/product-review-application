using ProductReviewApplication.Infrastructure.AzureTables.Entities;

namespace ProductReviewApplication.Mapping
{
    public class ProductReviewMapper
    {
        private static string DELIMITER = "_";
        public static ProductReview toReview(ProductReviewEntity productReviewEntity)
        {
            ProductReview review = new ProductReview
            {
                Review = productReviewEntity.Review,
                UserId = productReviewEntity.UserId,
                ProductName = productReviewEntity.PartitionKey
            };
            string[] splitKey = productReviewEntity.RowKey.Split(DELIMITER);
            review.ReviewDate = DateTimeOffset.FromUnixTimeMilliseconds(long.MaxValue - long.Parse(splitKey[0])).UtcDateTime;
            review.Id = splitKey[1];
            return review;
        }

        public static ProductReviewEntity toEntity(ProductReview productReview)
        {
            ProductReviewEntity entity = new ProductReviewEntity
            {
                Review = productReview.Review,
                UserId = productReview.UserId,
                PartitionKey = productReview.ProductName
            };
            long reviewTimestampInverted = long.MaxValue - DateTimeOffset.Now.ToUnixTimeMilliseconds();
            entity.RowKey = reviewTimestampInverted + DELIMITER + (productReview.Id ?? Guid.NewGuid().ToString());
            return entity;
        }
    }
}
