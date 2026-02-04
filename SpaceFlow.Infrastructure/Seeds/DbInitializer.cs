using Microsoft.AspNetCore.Identity;
using SpaceFlow.Core.Entities;
using SpaceFlow.Infrastructure.Data;
using System.Net.Http.Headers;

namespace SpaceFlow.Infrastructure.Seeds
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Add Roles
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            // Add Admin User
            if (!userManager.Users.Any(u => u.Email == "admin@spaceflow.com"))
            {
                var admin = new IdentityUser
                {
                    UserName = "admin@spaceflow.com",
                    Email = "admin@spaceflow.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin@123"); // password
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (!context.Rooms.Any())
            {
                var rooms = new List<Room>();

                // Single Rooms
                for (int i = 1; i <= 15; i++)
                {
                    rooms.Add(new Room
                    {
                        Name = $"Private Focus Pod {i}",
                        Description = "A quiet, high-tech space designed for maximum solo productivity.",
                        PricePerHour = 50 + (i * 2),
                        Capacity = 1,
                        ImageUrl = $"SingleRoom{i}.jpg",
                        IsAvailable = true
                    });
                }

                // Meeting Rooms
                for (int i = 1; i <= 17; i++)
                {
                    rooms.Add(new Room
                    {
                        Name = $"Executive Boardroom {i}",
                        Description = "Premium meeting space equipped with latest presentation tools.",
                        PricePerHour = 150 + (i * 10),
                        Capacity = i <= 10 ? 6 : 15,
                        ImageUrl = $"MeetingRoom{i}.jpg",
                        IsAvailable = true
                    });
                }

                context.Rooms.AddRange(rooms);
                await context.SaveChangesAsync();
            }
        }
    }
}