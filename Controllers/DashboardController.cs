using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Reviews.Interfaces;
using Reviews.Models;
using Reviews.ViewModels;
using System.Text;

namespace Reviews.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(
            IDashboardRepository dashboardRepository,
            IHttpContextAccessor httpContextAccessor,
            IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        

        public async Task<IActionResult> Index()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserByIdNoTracking(currentUserId);
            var userReviews = await _dashboardRepository.GetUserReviews();

            var dashboardViewModel = new DashboardViewModel()
            {
                Reviews = userReviews,
                AppUser = user
            };
            return View(dashboardViewModel);
        }
        //edit
        public async Task<IActionResult> UpdateProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user =  await _dashboardRepository.GetUserById (currentUserId);
            if(user == null)  return  View("Error");
            var updateUserViewModel = new UpdateUserViewModel
            {
                Id = currentUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };
            return View(updateUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateUserViewModel updateUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Profile.");
                return View("UpdateProfile", updateUserViewModel);
            }
            var user = await _dashboardRepository
                .GetUserByIdNoTracking(updateUserViewModel.Id);
            user.UserName = updateUserViewModel.UserName;
            user.FirstName = updateUserViewModel.FirstName;
            user.LastName = updateUserViewModel.LastName;

            _dashboardRepository.Update(user);
            return RedirectToAction("Index");
        }
    }
}
