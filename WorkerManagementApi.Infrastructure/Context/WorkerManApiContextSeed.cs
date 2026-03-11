using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Infrastructure.Context
{
    public static class WorkerManApiContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var administratorRole = new ApplicationRole();
            administratorRole.Name = "admin";

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var managerRole = new ApplicationRole();
            managerRole.Name = "manager";

            if (roleManager.Roles.All(r => r.Name != managerRole.Name))
            {
                await roleManager.CreateAsync(managerRole);
            }

            var workerRole = new ApplicationRole();
            workerRole.Name = "worker";

            if (roleManager.Roles.All(r => r.Name != workerRole.Name))
            {
                await roleManager.CreateAsync(workerRole);
            }

            var admin = new ApplicationUser
            {
                UserName = "administrator",
                Email = "adminmail@example.com",
                EmailConfirmed = true,
            };

            if (userManager.Users.All(u => u.UserName != admin.UserName))
            {
                await userManager.CreateAsync(admin, "Adminlogin@2");
                await userManager.AddToRolesAsync(admin, new[] { administratorRole.Name });
            }

        }

        public static async Task SeedSampleDataAsync(WorkerManApiContext context, UserManager<ApplicationUser> userManager)
        {
            var admin = userManager.Users.Where(x => x.UserName == "admin").FirstOrDefault();
            await context.SaveChangesAsync();
        }
    }
}
