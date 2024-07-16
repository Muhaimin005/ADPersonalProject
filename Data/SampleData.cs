using ADTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ADTest.Data
{
    public static class SampleData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            // Create a sample user (admin)
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var adminEmail = "admin@gmail.com"; // Customize email
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Name = "admin",
                    Email = adminEmail,
                    PhoneNumber = "0111111111",
                    CreatedAt = DateTime.UtcNow // Set creation timestamp
                };

                var password = new PasswordHasher<ApplicationUser>();
                var hashedPassword = password.HashPassword(adminUser, "Abc123?"); // Set desired password
                adminUser.PasswordHash = hashedPassword;

                await userManager.CreateAsync(adminUser);
            }

            // Assign roles to the user
            await AssignRoles(serviceProvider, adminEmail, ["Admin", "Student", "Lecturer"]);

            // Save changes to the database
            await context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            var userManager = services.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);
            return result;
        }
    }
}
