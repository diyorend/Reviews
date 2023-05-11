using Microsoft.AspNetCore.Mvc;
using Reviews.Interfaces;
using Reviews.Models;
using Reviews.ViewModels;

namespace Reviews.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewController(
            IReviewRepository reviewRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _reviewRepository = reviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Review> reviews = await _reviewRepository.GetAllAsync();
            return View(reviews);
        }

        //create
        public IActionResult Create()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createReviewViewModel = new CreateReviewViewModel { AppUserId = currentUserId };
            return View(createReviewViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var review = new Review
                {
                    Name = model.Name,
                    Description = model.Description,
                    Group = model.Group,
                    AppUserId = model.AppUserId
                };
                _reviewRepository.Add(review);
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");

            }
            ModelState.AddModelError("", "Updating is failed");
            return View(model);
        }
        //edit
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var review = await _reviewRepository.GetByIdAsync(id);
            if(review == null) 
                return View("Error");
            var reviewVM = new UpdateReviewViewModel
            {
                Id = id,
                Name = review.Name,
                Description = review.Description,
                Group = review.Group
            };
            return View(reviewVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,UpdateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit review.");
                return View("Edit", model);
            }
            var currentReview = await _reviewRepository.GetByIdAsyncNoTracking(id);
            if(currentReview != null)
            {
                currentReview.Name = model.Name;
                currentReview.Description = model.Description;
                currentReview.Group = model.Group;
                _reviewRepository.Update(currentReview);
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");   
            }
            return View(model);
        }
        //delete
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review != null)
                return View(review);
            return View("Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return View("Error");
            _reviewRepository.Delete(review);
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
