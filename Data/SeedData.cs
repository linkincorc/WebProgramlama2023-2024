using Microsoft.AspNetCore.Identity;
using PlaneTicket.Models;

namespace PlaneTicket.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminUser = "OgrenciNo@sakarya.edu.tr";
            string adminPassword = "Admin123!"; // Make sure to use a strong password in production

            if (await userManager.FindByNameAsync(adminUser) == null)
            {
                var user = new User
                {
                    UserName = adminUser,
                    Email = adminUser,
                    EmailConfirmed = true,
                    Name = "", 
                    Surname=""
                };

                IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }
    }
}