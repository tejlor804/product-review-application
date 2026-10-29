using ProductReviewApplication.Infrastructure.AzureTables.Entities;

namespace ProductReviewApplication.Repositories
{
    public interface IProductReviewRepository
    {
        List<ProductReviewEntity> GetReviewsOfProduct(String productName);
        ProductStateEntity? GetLatestReviewIdOfProduct(String productName);
        void SaveReview(ProductReview productReview);
        public Task SaveReviewAndUpdateLatestAsync(
            ProductReview productReview,
            ProductStateEntity currentStatus);
    }
}
