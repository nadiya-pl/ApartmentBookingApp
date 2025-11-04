using AuthAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        
        string[] roles = { "Admin", "Guest" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

       
        var adminEmail = "admin@demo.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "(demo user)",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

      
        var userEmail = "user@demo.com";
        var user = await userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            user = new AppUser
            {
                UserName = userEmail,
                Email = userEmail,
                FirstName = "Kristin",
                LastName = "Ott",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "User123!");
            await userManager.AddToRoleAsync(user, "Guest");
        }
    }
}
