using Microsoft.AspNetCore.Identity;

namespace TrackingPedidos.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            string[] roleNames = { "Administrador", "Cliente", "Despachador", "Transportista", "Distribuidor", "Mensajero" };

            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new ApplicationRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    roleManager.CreateAsync(role);
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("Tracker").Result == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Tracker";
                user.Email = "tracker@gmail.com";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                IdentityResult result = userManager.CreateAsync(user, "Tracker2019@").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrador").Wait();
                }
            }
        }
    }
}