using System.Diagnostics;
using makingticket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace makingticket.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Home/Index - Just a landing page
        public IActionResult Index()
        {
            return View();
        }

        // Signup GET
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        // Signup POST
        [HttpPost]
        public async Task<IActionResult> Signup(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // Default role
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Ticket"); // Redirect to the Ticket controller's Index view
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        // Login GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View();
                }

                var roles = await _userManager.GetRolesAsync(user);

                // Redirect based on role (SuperAdmin, Admin, User)
                if (roles.Contains("SuperAdmin") || roles.Contains("Admin"))
                    return RedirectToAction("Index", "Ticket"); // Admin and SuperAdmin redirected to Ticket Index
                else
                    return RedirectToAction("Index", "Ticket"); // Normal User redirected to Ticket Index (could be different in your setup)
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
