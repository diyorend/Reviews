using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reviews.Data;
using Reviews.Models;
using Reviews.ViewModels;

namespace Reviews.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        //register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if(!await _roleManager.RoleExistsAsync("owner"))
            {
                await _roleManager.CreateAsync(new IdentityRole("owner"));
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("user"));
            }
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "owner",
                Text = "owner"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "admin",
                Text = "admin"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "user",
                Text = "user"
            });
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.RoleList = listItems;
            return View(registerViewModel);
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
                    if(registerViewModel.RoleSelected != null &&
                        registerViewModel.RoleSelected.Length > 0 &&
                        registerViewModel.RoleSelected == "owner")
                    {
                        await _userManager.AddToRoleAsync(newUser, "owner");
                    }
                    else if(registerViewModel.RoleSelected != null &&
                        registerViewModel.RoleSelected == "admin")
                    {
                        await _userManager.AddToRoleAsync(newUser, "admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, "user");
                    }
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
            return RedirectToAction("Index", "Home");
        }
    }
}
