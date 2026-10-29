using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ProductReviewApplication.DTOs;
using ProductReviewApplication.Services;
using System.Collections;

namespace ProductReviewApplication.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class ProductController(IProductReviewService productReviewService) : ControllerBase
    {
        private const string GetReviewsRoute = "GetProductReviews";
        private readonly IProductReviewService _productReviewService = productReviewService;

        [HttpGet("{name}/reviews", Name = GetReviewsRoute)]
        public List<ProductReview> GetReviews(string name, [FromQuery] string userId)
        {
            return _productReviewService.GetReviewsOfProduct(name, userId);
        }

        [HttpPost("{name}/reviews")]
        public async Task<ActionResult<ProductReview>> CreateReview(string name, [FromBody] ProductReviewRequest request)
        {
            await _productReviewService.CreateReview(name, request);
            return CreatedAtRoute(
                GetReviewsRoute,
                new { name, userId = request.UserId },
                null);
        }

    }
}
