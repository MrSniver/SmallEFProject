using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Tasks.Repositories;
using WorkerManagementApi.Application.Tokens.Interfaces;
using WorkerManagementApi.Application.Tokens.Services;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Infrastructure.Context;
using WorkerManagementApi.Infrastructure.Repositories.ApplicationUsers;
using WorkerManagementApi.Infrastructure.Repositories.Common;
using WorkerManagementApi.Infrastructure.Repositories.Tasks;
using WorkerManagementApi.Infrastructure.Services;

namespace WorkerManagementApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
        {
            services.AddScoped<IWorkerManApiContext>(provider => provider.GetService<WorkerManApiContext>());
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IApplicationRoleRepository, ApplicationRoleRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IIdentityCheckService, IdentityCheckService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<WorkerManApiContext>();

            services.Configure<IdentityOptions>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#$^%";
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 2;
                });

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                    .UseDbContext<WorkerManApiContext>();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("/connect/token")
                   .AllowPasswordFlow()
                   .AllowRefreshTokenFlow()
                   .AddDevelopmentSigningCertificate()
                   .AddDevelopmentEncryptionCertificate();

                    options.RegisterScopes("openid", "profile", "offline_access");

                    options.SetAuthorizationEndpointUris("/connect/authorize");

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            services.AddScoped<ITokenService, TokenService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("FullAccess", policy => policy.RequireRole("admin"));
                options.AddPolicy("Read", policy => policy.RequireRole("admin", "manager", "worker"));
                options.AddPolicy("Write", policy => policy.RequireRole("admin", "manager", "worker"));
                options.AddPolicy("Delete", policy => policy.RequireRole("admin", "manager"));
            });

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
