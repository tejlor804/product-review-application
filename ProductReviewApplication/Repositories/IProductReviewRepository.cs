using ProductReviewApplication.Infrastructure.AzureTables.Entities;

namespace ProductReviewApplication.Repositories
{
    public interface IProductReviewRepository
    {
        List<ProductReviewEntity> GetReviewsOfProduct(String productName);
        ProductReviewEntity? GetLatestReviewOfProduct(String productName);
        void SaveReview(ProductReview productReview);
    }
}
