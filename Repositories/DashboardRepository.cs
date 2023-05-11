using Microsoft.EntityFrameworkCore;
using Reviews.Data;
using Reviews.Interfaces;
using Reviews.Models;

namespace Reviews.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public DashboardRepository(DataContext context, IHttpContextAccessor httpContext) 
        {
            _context = context;
            _httpContext = httpContext;
        }

        public bool Delete(AppUser appUser)
        {
            _context.Remove(appUser);
            return Save();
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<AppUser> GetUserByIdNoTracking(string userId)
        {
            return await _context.Users.Where(u => u.Id == userId)
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<Review>> GetUserReviews()
        {
            var currentUserId = _httpContext.HttpContext?.User.GetUserId();
            var userReviews = await _context.Reviews
                .Where(r => r.AppUser.Id == currentUserId).ToListAsync();
            return userReviews;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0?true : false;
        }

        public bool Update(AppUser appUser)
        {
            _context.Users.Update(appUser);
            return Save();
        }
    }
}
