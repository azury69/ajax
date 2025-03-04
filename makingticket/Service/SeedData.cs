using Microsoft.AspNetCore.Identity;
using makingticket.Models;
using Microsoft.EntityFrameworkCore;

namespace makingticket.Service
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            // Remove all roles and users before seeding fresh data
            //var roles = await roleManager.Roles.ToListAsync();
            //foreach (var role in roles)
            //{
            //    await roleManager.DeleteAsync(role);
            //}

            //var users = await userManager.Users.ToListAsync();
            //foreach (var user in users)
            //{
            //    await userManager.DeleteAsync(user);
            //}
            var roleNames = new List<string> { "SuperAdmin", "Admin", "User" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            var superAdminEmail = "superadmin@infinite.com";
            var superAdminPassword = "SuperAdmin@1";
            var adminEmail = "admin@infinite.com";
            var adminPassword = "Admin@1";
            var userEmail = "user@infinite.com";
            var userPassword = "User@1";
            await CreateUser(userManager, superAdminEmail, superAdminPassword, "SuperAdmin", null);
            await CreateUser(userManager, adminEmail, adminPassword, "Admin",superAdminEmail);
            await CreateUser(userManager, userEmail, userPassword, "User",adminEmail);
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, string password, string role, string? assignedByEmail)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {

                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                if (role != "SuperAdmin" && assignedByEmail != null)
                {
                    var assignedByUser = await userManager.FindByEmailAsync(assignedByEmail);
                    user.AssignedById = assignedByUser?.Id;
                }
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (!await userManager.IsInRoleAsync(user, role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user {email}: {error.Description}");
                    }
                }
            }
            else
            {
                // Ensure the user is assigned to the correct role in case they exist but are missing a role.
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                if (role != "SuperAdmin" && assignedByEmail != null && user.AssignedById == null)
                {
                    var assignedByUser = await userManager.FindByEmailAsync(assignedByEmail);
                    user.AssignedById = assignedByUser?.Id;
                    await userManager.UpdateAsync(user);
                }
            }


        }
    }
}
