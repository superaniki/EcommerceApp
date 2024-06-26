using EcommerceApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EcommerceApp.Data
{
    public static class DbInitializer
    {

        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await Initialize(context, userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }

        private static async Task seedUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, String username, String email, String password, string role)
        {
            // Seed admin user
            var appUser = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            var user = await userManager.FindByEmailAsync(appUser.Email);

            if (user == null)
            {
                var createUser = await userManager.CreateAsync(appUser, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, role);

                    // Create an empty Customer record for the new user
                    var customer = new Customer
                    {
                        User = appUser,
                        FirstName = "John",
                        LastName = "Doe"
                    };

                    // Add the Customer entity to the context
                    context.Customers.Add(customer);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Apply any pending migrations
            context.Database.Migrate();

            // Seed roles
            string[] roleNames = { "Administrator", "Customer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed admin user
            await seedUser(context, userManager, "admin", "admin@example.com", "Admin@123", "Administrator");
            //await seedUser(userManager, "rickard", "customer@gmail.com", "Customer@123", "Customer");
        }
    }
}
