using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DAL
{
    public static class RoleAndUserSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roles = { "librarian", "reader" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Startowe konto bibliotekarza w Dockerze
            var librarianEmail = "librarian@library.local";
            if (await userManager.FindByEmailAsync(librarianEmail) == null)
            {
                var librarian = new User
                {
                    UserName = librarianEmail,
                    Email = librarianEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(librarian, "Librarian123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, "librarian");
                }
            }
        }
    }
}
