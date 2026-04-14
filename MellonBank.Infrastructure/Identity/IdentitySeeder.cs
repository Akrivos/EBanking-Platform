using MellonBank.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MellonBank.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = [RoleType.Staff.ToString(), RoleType.Customer.ToString()];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to create user roles.");
                    }
                }
            }

            var adminUserName = configuration["SeedSettings:StaffUserName"];
            var adminEmail = configuration["SeedSettings:StaffEmail"];
            var adminPassword = configuration["SeedSettings:StaffPassword"];

            var existingUser = await userManager.FindByNameAsync(adminUserName!);

            if (existingUser is null)
            {
                var staffUser = new ApplicationUser(
                    firstName: "Akis",
                    lastName: "Patatakis",
                    address: "Random Address 31",
                    afm: "987654321")
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    PhoneNumber = "2100000000",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var createResult = await userManager.CreateAsync(staffUser, adminPassword!);

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to seed the staff user");
                }

                existingUser = staffUser;
            }

            if (!await userManager.IsInRoleAsync(existingUser, RoleType.Staff.ToString()))
            {
                var roleResult = await userManager.AddToRoleAsync(existingUser, RoleType.Staff.ToString());

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to assign the staff role.");
                }
            }
        }
    }
}
