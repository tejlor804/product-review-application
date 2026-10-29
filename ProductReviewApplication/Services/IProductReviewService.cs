using ProductReviewApplication.DTOs;

namespace ProductReviewApplication.Services
{
    public interface IProductReviewService
    {
        List<ProductReview> GetReviewsOfProduct(string name, string userId);
        void CreateReview(string productName, ProductReviewRequest request);
    }
}
