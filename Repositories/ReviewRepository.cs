using Microsoft.EntityFrameworkCore;
using Reviews.Data;
using Reviews.Interfaces;
using Reviews.Models;

namespace Reviews.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public bool Add(Review review)
        {
            _context.Reviews.Add(review);
            return Save();
        }

        public bool Delete(Review review)
        {
            _context.Reviews.Remove(review);
            return Save();
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.Include(review => review.Comments)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Reviews.Include(review => review.Comments)
                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetReviewsBySearch(string searchedText)
        {
            return await _context.Reviews.Where(r => 
                r.Name.Contains(searchedText)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Review review)
        {
            _context.Reviews.Update(review); 
            return Save();
        }
    }
}
