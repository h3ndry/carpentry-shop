
using CarpentryShop.Areas.Identity.Data;
using CarpentryShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class SampleData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<CarpentryShopIdentityDbContext>();

        string[] roles = new string[] { "Superuser", "Administrator", "Customer"};

        foreach (string role in roles)
        {
            var roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == role))
            {
                var oRole = new IdentityRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                };
                var result = roleStore.CreateAsync(oRole).Result;
            }
        }

        var user = new IdentityUser
        {
            // FirstName = "Admin",
            // LastName = "User",
            Email = "admin@localhost.com",
            UserName = "admin@localhost.com",
            // NormalizedEmail = "ADMIN@COPALCOR.CO.ZA",
            // NormalizedUserName = "ADMIN@COPALCOR.CO.ZA",
            // PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            LockoutEnabled = false
            // SecurityStamp = Guid.NewGuid().ToString("D")
            //
        };


        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            UserManager<IdentityUser> _userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var results = await _userManager.CreateAsync(user, "P@$$w0r1d");
        }

        await AssignRoles(serviceProvider, user.Email, roles);
        await context.SaveChangesAsync();
    }

    public static async Task<IdentityUser> AssignRoles(IServiceProvider services, string email, string[] roles)
    {
        UserManager<IdentityUser> _userManager = services.GetService<UserManager<IdentityUser>>();

        IdentityUser user = await _userManager.FindByEmailAsync(email);
        // var result = await _userManager.AddToRoleAsync(user, roles[0]);
        // var result = await _userManager.AddToRoleAsync(user, roles[0]);

        return user;
    }
}
