using Microsoft.AspNetCore.Identity;
using MVCAngularShortener.Data;

namespace MVCAngularShortener.Infrastructure
{
    public class SeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public SeedData(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public  async Task SeedAdminRole()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                await _roleManager.CreateAsync(adminRole);
            }

            if (await _userManager.FindByNameAsync("admin") == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com"
                };

                await _userManager.CreateAsync(adminUser, "Password");

                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
