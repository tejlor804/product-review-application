namespace ProductReviewApplication.Repositories
{
    public interface IUserRepository
    {
        void SaveLatestReadReviewIdOfUser(string userId, string productName, string reviewId);
        string? GetLatestReadReviewIdOfUser(string productName, string userId);
    }
}
