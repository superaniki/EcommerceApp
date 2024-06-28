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


        private static async Task SeedUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, String username, String email, String password, string role)
        {

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Seed admin user
                var appUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var createUser = await userManager.CreateAsync(appUser, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, role);

                    user = await userManager.FindByEmailAsync(email);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var result = await userManager.ConfirmEmailAsync(user, code);

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

        private static async Task SeedProducts(ApplicationDbContext context)
        {
            // Check if any products exist
            if (!context.Products.Any())
            {
                // Create a list of products
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Fresh Strawberries",
                        Slug = "fresh-strawberries",
                        Description = "Juicy and sweet fresh strawberries, perfect for snacking or baking.",
                        Price = 3.99m,
                        Quantity = 100,
                        ImageUrl = "https://images.unsplash.com/photo-1518635017498-87f514b751ba",
                    },
                    new Product
                    {
                        Name = "Strawberry Jam",
                        Slug = "strawberry-jam",
                        Description = "Homemade strawberry jam made with ripe, sweet strawberries.",
                        Price = 5.99m,
                        Quantity = 50,
ImageUrl = "https://plus.unsplash.com/premium_photo-1689344314069-b60bf06d564c"                    },
                    new Product
                    {
                        Name = "Strawberry Smoothie Mix",
                        Slug = "strawberry-smoothie-mix",
                        Description = "A delicious blend of strawberries, yogurt, and honey for a refreshing smoothie.",
                        Price = 7.99m,
                        Quantity = 75,
ImageUrl = "https://images.unsplash.com/photo-1692284014113-cc8466ca8688"                    },
                    new Product
                    {
                        Name = "Strawberry Cheesecake",
                        Slug = "strawberry-cheesecake",
                        Description = "Creamy cheesecake with a sweet strawberry topping.",
                        Price = 19.99m,
                        Quantity = 25,
ImageUrl = "https://plus.unsplash.com/premium_photo-1689344314549-2540cc7e2d23"                    },
                    new Product
                    {
                        Name = "Strawberry Lemonade",
                        Slug = "strawberry-lemonade",
                        Description = "Refreshing lemonade with a sweet strawberry twist.",
                        Price = 2.99m,
                        Quantity = 100,
ImageUrl = "https://images.unsplash.com/photo-1692285958443-825c9f5af9bb"                    },
                    new Product
                    {
                        Name = "Strawberry Shortcake",
                        Slug = "strawberry-shortcake",
                        Description = "Classic dessert with sweet strawberries and whipped cream.",
                        Price = 12.99m,
                        Quantity = 30,
ImageUrl = "https://images.unsplash.com/photo-1518635017498-87f514b751ba"                    },
                    new Product
                    {
                        Name = "Strawberry Vinegar",
                        Slug = "strawberry-vinegar",
                        Description = "Fruity vinegar made with ripe strawberries, perfect for salad dressings.",
                        Price = 6.99m,
                        Quantity = 40,
ImageUrl = "https://images.unsplash.com/photo-1518635017498-87f514b751ba"                    },
                    new Product
                    {
                        Name = "Strawberry Syrup",
                        Slug = "strawberry-syrup",
                        Description = "Sweet strawberry syrup for pancakes, waffles, and more.",
                        Price = 4.99m,
                        Quantity = 60,
ImageUrl = "https://images.unsplash.com/photo-1518635017498-87f514b751ba"                    },
                    new Product
                    {
                        Name = "Strawberry Salsa",
                        Slug = "strawberry-salsa",
                        Description = "Refreshing salsa with strawberries, perfect for chips or grilled meats.",
                        Price = 5.99m,
                        Quantity = 70,
ImageUrl = "https://images.unsplash.com/photo-1518635017498-87f514b751ba"                    },
                    new Product
                    {
                        Name = "Strawberry Balsamic Glaze",
                        Slug = "strawberry-balsamic-glaze",
                        Description = "Sweet and tangy glaze made with strawberries and balsamic vinegar.",
                        Price = 7.99m,
                        Quantity = 50,
ImageUrl = "https://images.unsplash.com/photo-1518635017498-87f514b751ba"                    }
                };

                // Add the products to the context
                await context.Products.AddRangeAsync(products);

                // Save changes to the database
                await context.SaveChangesAsync();
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
            await SeedUser(context, userManager, "admin", "admin@example.com", "1q2w#E¤R5t6y", "Administrator");
            await SeedUser(context, userManager, "rickard", "customer@gmail.com", "Customer@123", "Customer");

            await SeedProducts(context);

        }
    }
}
