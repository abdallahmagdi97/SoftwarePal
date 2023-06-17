using Microsoft.EntityFrameworkCore;
using SoftwarePal.Data;
using SoftwarePal.Models;

namespace SoftwarePal.Repositories
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ReviewsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Review> GetApprovedReviewsByProductId(int productId)
        {
            return _dbContext.Reviews
                .Where(r => r.ProductId == productId && r.IsApproved == true)
                .ToList();
        }

        public async Task AddReview(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
        }

        public void UpdateReview(Review review)
        {
            _dbContext.Reviews.Update(review);
            _dbContext.SaveChanges();
        }

        public void DeleteReview(int reviewId)
        {
            var review = _dbContext.Reviews.Find(reviewId);
            if (review != null)
            {
                _dbContext.Reviews.Remove(review);
                _dbContext.SaveChanges();
            }
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _dbContext.Reviews
                .ToListAsync();
        }

        public async Task<List<Review>> GetApprovedReviews()
        {
            return await _dbContext.Reviews
                .Where(r => r.IsApproved == true)
                .ToListAsync();
        }

        public async Task ApproveReview(int reviewId)
        {
            var review = await _dbContext.Reviews.FindAsync(reviewId);
            if (review == null)
                throw new Exception("Review not found");
            review.IsApproved = true;
            UpdateReview(review);
        }
    }
    public interface IReviewsRepository
    {
        List<Review> GetApprovedReviewsByProductId(int productId);
        Task<List<Review>> GetAllReviews();
        Task<List<Review>> GetApprovedReviews();
        Task AddReview(Review review);
        void UpdateReview(Review review);
        void DeleteReview(int reviewId);
        Task ApproveReview(int reviewId);
    }
}
