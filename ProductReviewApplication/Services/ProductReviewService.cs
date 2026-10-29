using Azure;
using Azure.Core;
using ProductReviewApplication.DTOs;
using ProductReviewApplication.Exceptions;
using ProductReviewApplication.Infrastructure.AzureTables.Entities;
using ProductReviewApplication.Mapping;
using ProductReviewApplication.Repositories;
using System.Xml.Linq;

namespace ProductReviewApplication.Services
{
    public class ProductReviewService(IProductReviewRepository productReviewRepository, IUserRepository userRepository) : IProductReviewService
    {
        IProductReviewRepository productReviewRepository = productReviewRepository;
        IUserRepository userRepository = userRepository;

        public async Task CreateReview(string productName, ProductReviewRequest request)
        {
            ProductReviewRequest.Validate(request);
            if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentException("Product name must be defined!");
            ProductStateEntity? latestReviewOfProduct = productReviewRepository.GetLatestReviewIdOfProduct(productName);
            String? latestReviewIdSeenByUser = userRepository.GetLatestReadReviewIdOfUser(productName, request.UserId);
            if (latestReviewOfProduct != null && !string.IsNullOrEmpty(latestReviewOfProduct.LatestReviewId) &&
                (latestReviewIdSeenByUser == null || !latestReviewIdSeenByUser.Equals(latestReviewOfProduct.LatestReviewId)))
            {
                throw new NewestReviewNotReadException();
            }
            try
            {
                await productReviewRepository.SaveReviewAndUpdateLatestAsync(new() { ProductName = productName, UserId = request.UserId, Review = request.Review }, latestReviewOfProduct);
            }
            catch (RequestFailedException ex) when (ex.Status == 412)
            {
                throw new NewestReviewNotReadException();
            }
        }

        public List<ProductReview> GetReviewsOfProduct(string productName, String userId)
        {
            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("Request parameters must be provided!");
            }
            List<ProductReviewEntity> reviewsOfProduct = productReviewRepository.GetReviewsOfProduct(productName);
            if (reviewsOfProduct.Count > 0)
            {
                userRepository.SaveLatestReadReviewIdOfUser(userId, productName, reviewsOfProduct.First().RowKey);
            }
            return [.. reviewsOfProduct.Select(entity => ProductReviewMapper.toReview(entity))];
        }
    }
}
