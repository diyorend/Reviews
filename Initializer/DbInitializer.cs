using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reviews.Data;
using Reviews.Interfaces;
using Reviews.Models;

namespace Reviews.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;

        public DbInitializer(DataContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }
        public async void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            _roleManager.CreateAsync(new IdentityRole(UserRoles.Owner))
                .GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin))
                .GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(UserRoles.User))
                .GetAwaiter().GetResult();

            _userManager.CreateAsync(new AppUser
            {
                UserName = "diyorend",
                Email = "diyorend@gmail.com",
                EmailConfirmed = true,
                FirstName = "Mukhammadislom",
            }, "Owner@123").GetAwaiter().GetResult();
            AppUser user = _context.Users.Where(u => u.Email == "diyorend@gmail.com")
                .FirstOrDefault();
            _userManager.AddToRoleAsync(user, UserRoles.Owner).GetAwaiter().GetResult();    
        }
    }
}
