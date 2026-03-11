using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkerManagementApi.Application.ApplicationUsers.Services;
using WorkerManagementApi.Application.Common.Behaviours;
using WorkerManagementApi.Application.Tasks.Services;
using WorkerManagementApi.Application.Tokens.Interfaces;
using WorkerManagementApi.Application.Tokens.Services;

namespace WorkerManagementApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}
