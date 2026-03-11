using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WorkerManagementApi.Application;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Tokens.Dtos;
using WorkerManagementApi.Infrastructure;
using WorkerManagementApi.Infrastructure.Context;
using WorkerManagementApi.Infrastructure.Extensions;
using WorkerManagementApi.Infrastructure.Services;
using WorkerManagementApi.Middlewares.ExceptionHandling;
using WorkerManagementApi.Middlewares.Tokens;
using WorkerManagementApi.Middlewares.User;

namespace WorkerManagementApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Worker management API", Version = "v1" });
            });

            services.AddDbContext<WorkerManApiContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"],
                x => x.MigrationsAssembly(typeof(WorkerManApiContext).Assembly.FullName)));

            services.Configure<TokenDto>(Configuration.GetSection("token"));

            services.AddLocalization();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ExceptionHandlingMiddleware>();

            services.AddInfrastructure();
            services.AddControllers();
            services.AddApplication();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "WorkerMan API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Description = "Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkerMan API");
                });
            }

            app.UseRequestLocalization("pl-PL");
            app.UseOpenApi();
            app.UseSwagger();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors();
            app.IdentityDbIsCreated();
            app.SeedIdentityDataAsync();
            app.SeedDefualtSampleDataAsync();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<TokenMiddleware>();
            app.UseMiddleware<CurrentUserMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
