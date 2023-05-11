using Microsoft.EntityFrameworkCore;
using Reviews.Data;
using Reviews.Interfaces;
using Reviews.Models;

namespace Reviews.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public bool Add(AppUser appUser)
        {
            _context.Users.Add(appUser);
            return Save();
        }

        public bool Delete(AppUser appUser)
        {
            _context.Users.Update(appUser);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser appUser)
        {
            _context.Update(appUser);
            return Save();
        }
    }
}
