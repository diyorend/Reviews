using Reviews.Models;

namespace Reviews.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetByIdAsync(string id);
        bool Add(AppUser appUser);
        bool Update(AppUser appUser);
        bool Delete(AppUser appUser);
        bool Save();
    }
}
