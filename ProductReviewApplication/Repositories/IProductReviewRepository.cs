using ProductReviewApplication.Infrastructure.AzureTables.Entities;

namespace ProductReviewApplication.Repositories
{
    public interface IProductReviewRepository
    {
        Task<List<ProductReviewEntity>> GetReviewsOfProduct(String productName, int pageSize);
        ProductStateEntity? GetLatestReviewIdOfProduct(String productName);
        void SaveReview(ProductReview productReview);
        public Task SaveReviewAndUpdateLatestAsync(
            ProductReview productReview,
            ProductStateEntity currentStatus);
    }
}
