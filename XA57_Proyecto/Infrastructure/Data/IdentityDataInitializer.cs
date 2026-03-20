using Microsoft.AspNetCore.Identity;
using XA57_Proyecto.Domain.Entities;

namespace XA57_Proyecto.Infrastructure.Data
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdminUser(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrador"));
            }

            if (!await roleManager.RoleExistsAsync("Cliente"))
            {
                await roleManager.CreateAsync(new IdentityRole("Cliente"));
            }
        }

        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@xa57.com") == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@xa57.com",
                    Email = "admin@xa57.com",
                    Nombre = "Admin",
                    Apellido = "XA57"
                };

                IdentityResult result = await userManager.CreateAsync(user, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrador");
                }
            }
        }
    }
}
