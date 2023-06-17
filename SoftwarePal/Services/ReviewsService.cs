using SoftwarePal.Models;
using SoftwarePal.Repositories;

namespace SoftwarePal.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;

        public ReviewsService(IReviewsRepository reviewsRepository)
        {
            _reviewsRepository = reviewsRepository;
        }

        public List<Review> GetApprovedReviewsByProductId(int productId)
        {
            return _reviewsRepository.GetApprovedReviewsByProductId(productId);
        }
        public async Task<List<Review>> GetApprovedReviews()
        {
            return await _reviewsRepository.GetApprovedReviews();
        }
        public async Task<List<Review>> GetAllReviews()
        {
            return await _reviewsRepository.GetAllReviews();
        }
        public async Task AddReview(Review review)
        {
            await _reviewsRepository.AddReview(review);
        }

        public void UpdateReview(Review review)
        {
            _reviewsRepository.UpdateReview(review);
        }

        public void DeleteReview(int reviewId)
        {
            _reviewsRepository.DeleteReview(reviewId);
        }

        public async Task ApproveReview(int reviewId)
        {
            await _reviewsRepository.ApproveReview(reviewId);
        }
    }

    public interface IReviewsService
    {
        List<Review> GetApprovedReviewsByProductId(int productId);
        Task<List<Review>> GetApprovedReviews();
        Task<List<Review>> GetAllReviews();
        Task AddReview(Review review);
        void UpdateReview(Review review);
        void DeleteReview(int reviewId);
        Task ApproveReview(int reviewId);
    }
}
