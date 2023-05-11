using Reviews.Models;

namespace Reviews.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetReviewsBySearch(string searchedText);
        Task<Review> GetByIdAsync(int id);
        Task<Review> GetByIdAsyncNoTracking(int id);
        bool Add(Review review);
        bool Update(Review review);
        bool Delete(Review review);
        bool Save();
    }
}
