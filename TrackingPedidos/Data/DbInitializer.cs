using Microsoft.AspNetCore.Identity;

namespace TrackingPedidos.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Administrador", "Cliente" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("Tracker").Result == null)
            {
                var user = new IdentityUser();
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