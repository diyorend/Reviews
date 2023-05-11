using Reviews.Models;

namespace Reviews.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Review>> GetUserReviews();
        Task<AppUser> GetUserById(string userId);
        Task<AppUser> GetUserByIdNoTracking(string userId);
        bool Update(AppUser appUser);
        bool Delete(AppUser appUser);
        bool Save();
    }
}
