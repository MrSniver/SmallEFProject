using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Infrastructure.Context;

namespace WorkerManagementApi.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void IdentityDbIsCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbContext = services.GetRequiredService<WorkerManApiContext>();

                dbContext.Database.Migrate();
            }
        }

        public static async Task SeedIdentityDataAsync(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                await WorkerManApiContextSeed.SeedDefaultUserAsync(userManager, roleManager);

            }
        }

        public static async Task SeedDefualtSampleDataAsync(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbContext = services.GetRequiredService<WorkerManApiContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await WorkerManApiContextSeed.SeedSampleDataAsync(dbContext, userManager);
            }
        }
    }
}
