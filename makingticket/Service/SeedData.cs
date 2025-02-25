﻿using Microsoft.AspNetCore.Identity;
using makingticket.Models;

namespace makingticket.Service
{
    public class SeedData
    {
        //public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    using (var scope = serviceProvider.CreateScope())
        //    {

        //        var roleNames = new List<string> { "SuperUser", "AdminUser", "User" };

        //        foreach (var role in roleNames)
        //        {
        //            var roleExist = await roleManager.RoleExistsAsync(role);

        //            if (!roleExist)
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(role));
        //            }


        //            var adminEmail = "admin@infinite.com";
        //            var adminPassword = "Admin@1";

        //            var admin = await userManager.FindByEmailAsync(adminEmail);
        //            if (admin == null)
        //            {
        //                admin = new IdentityUser
        //                {
        //                    UserName = adminEmail,
        //                    Email = adminEmail,

        //                };

        //                var result = await userManager.CreateAsync(admin, adminPassword);

        //                if (result.Succeeded)
        //                {
        //                    await userManager.AddToRoleAsync(admin, "SuperUser");
        //                }
        //            }

        //            var adminUserEmail = "adminuser@infinite.com";
        //            var adminUserPassword = "Adminuser@1";

        //            var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
        //            if (adminUser == null)
        //            {
        //                adminUser = new IdentityUser
        //                {
        //                    UserName = adminUserEmail,
        //                    Email = adminUserEmail,

        //                };

        //                var result = await userManager.CreateAsync(adminUser, adminUserPassword);

        //                if (result.Succeeded)
        //                {
        //                    await userManager.AddToRoleAsync(adminUser, "AdminUser");
        //                }
        //            }

        //            //creating superadmin
        //            var UserEmail = "user@infinite.com";
        //            var UserPassword = "User@1";

        //            var User = await userManager.FindByEmailAsync(UserEmail);
        //            if (User == null)
        //            {
        //                User = new IdentityUser
        //                {
        //                    UserName = UserEmail,
        //                    Email = UserEmail,

        //                };

        //                var result = await userManager.CreateAsync(User, UserPassword);

        //                if (result.Succeeded)
        //                {
        //                    await userManager.AddToRoleAsync(User, "User");
        //                }
        //            }



        //        }
        //    }
        //}

        public static async Task Initialize(IServiceProvider services, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new List<string> { "SuperAdmin", "Admin", "User" };

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }


            var superAdminEmail = "superadmin@infinite.com";
            var superAdminPassword = "SuperAdmin@1";
            var adminEmail = "admin@infinite.com";
            var adminPassword = "Admin@1";
            var userEmail = "user@infinite.com";
            var userPassword = "User@1";
            await CreateUser(userManager, superAdminEmail, superAdminPassword, "SuperAdmin");
            await CreateUser(userManager, adminEmail, adminPassword, "Admin");
            await CreateUser(userManager, userEmail, userPassword, "User");
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, string password, string role)
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
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }


        }
    }
}
