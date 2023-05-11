using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reviews.Interfaces;
using Reviews.Models;
using Reviews.Repositories;
using Reviews.ViewModels;

namespace Reviews.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<AppUser> _roleManager;

        public UserController(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<AppUser> roleManager)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsersAsync();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    ProfileImagesUrl = user.ProfileImagesUrl,
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            var userViewModel = new UserDetailViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                ProfileImagesUrl = user.ProfileImagesUrl,
                Reviews = user.Reviews
            };
            return View(userViewModel);
        }
    }    
}
