using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using makingticket.Models;
using System.Linq;
using System.Threading.Tasks;

namespace makingticket.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Index - Show the list of users and their roles
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList(); // Get all users
            var usersWithRoles = new List<(ApplicationUser user, IList<string> roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Get roles for each user
                if (!roles.Contains("SuperAdmin"))
                {
                    usersWithRoles.Add((user, roles));

                }
            }

            // Get the ctly logged-in user (admin)
            var currentUser = await _userManager.GetUserAsync(User);

            // Create the ViewModel to pass both data
            var model = new RoleManagementViewModel
            {
                UsersWithRoles = usersWithRoles,
                CurrentUser = currentUser
            };

            return View(model); // Pass the model to the view
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string targetUserId, string newRole)
        {
            var user = await _userManager.GetUserAsync(User);  // Get the current logged-in admin user

            if (user == null)
            {
                // Handle the case where the user is not logged in
                return RedirectToAction("Login", "Home");
            }

            var targetUser = await _userManager.FindByIdAsync(targetUserId);

            if (targetUser != null)
            {
                // Check if the admin is trying to change their own role
                if (targetUser.Id == user.Id)
                {
                    // Prevent admin from changing their own role
                    ModelState.AddModelError("", "You cannot change your own role.");
                }
                // Check if the admin is trying to change another admin's role
                else if (await _userManager.IsInRoleAsync(targetUser, "Admin"))
                {
                    // Prevent admin from changing another admin's role unless they assigned them
                    if (targetUser.AssignedById != user.Id)
                    {
                        ModelState.AddModelError("", "You cannot change the role of another admin that you did not assign.");
                    }
                    else
                    {
                        // Admin can change the role of the other admin they assigned
                        var currentRoles = await _userManager.GetRolesAsync(targetUser);
                        await _userManager.RemoveFromRolesAsync(targetUser, currentRoles); // Remove current roles
                        await _userManager.AddToRoleAsync(targetUser, newRole); // Assign the new role
                    }
                }
                else
                {
                    // Admin can change the role of non-admin users (normal users)
                    var currentRoles = await _userManager.GetRolesAsync(targetUser);
                    await _userManager.RemoveFromRolesAsync(targetUser, currentRoles); // Remove current roles
                    await _userManager.AddToRoleAsync(targetUser, newRole); // Assign the new role
                }
            }

            return RedirectToAction("Index");
        }
    }
}
