using ProductReviewApplication.DTOs;

namespace ProductReviewApplication.Services
{
    public interface IProductReviewService
    {
        Task<List<ProductReview>> GetReviewsOfProduct(string name, string userId, int? pagesize);
        Task CreateReview(string productName, ProductReviewRequest request);
    }
}
