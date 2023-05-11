using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reviews.Data;
using Reviews.Models;
using Reviews.ViewModels;

namespace Reviews.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        //register
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
                return View(registerViewModel);
            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if(user == null)
            {
                var newUser = new AppUser()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.UserName
                };

                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                    return RedirectToAction("Index", "Home");
                }
                return View(registerViewModel);
            }
            TempData["Error"] = "Email is already in use!";
            return View(registerViewModel);
        }
        //Login
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            { 
                return View(loginViewModel);
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager
                    .CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager
                        .PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Wrong Password";
                return View(loginViewModel);
            }
            TempData["Error"] = "Email is not registered!";
            return View(loginViewModel);
        }
        //logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register", "Account");
        }
    }
}
