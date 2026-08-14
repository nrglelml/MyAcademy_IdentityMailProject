using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityMail.Web.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            string[] roles = { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }


            var adminEmail = configuration["AdminSeed:Email"];
            var adminPassword = configuration["AdminSeed:Password"];

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Sistem",
                    LastName = "Admin",
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                   
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Admin kullanıcı oluşturulamadı: {errors}");
                }
            }
            else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
              
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
