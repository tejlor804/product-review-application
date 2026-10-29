using Azure.Core;

namespace ProductReviewApplication.DTOs
{
    public class ProductReviewRequest
    {
        public string? Review { get; set; }
        public string? UserId { get; set; }
        public static void Validate(ProductReviewRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("A request body is required!");
            }
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Review))
            {
                throw new ArgumentException("Request parameters must be provided!");
            }
            if (request.Review.Length > 500)
            {
                throw new ArgumentException("Review should not be longer than 500 characters!");
            }
        }
    }
}
