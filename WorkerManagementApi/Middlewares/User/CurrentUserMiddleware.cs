using Microsoft.AspNetCore.Authorization;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Tokens.Interfaces;

namespace WorkerManagementApi.Middlewares.User
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext httpContext, IApplicationUserRepository applicationUserRepository, ITokenService tokenService)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                await _next(httpContext);
                return;
            }
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = tokenService.ValidateJwtToken(token);
            if (userId == null) 
            {
                throw new UnauthorizedAccessException();
            }
            
            await _next(httpContext);
        }
    }
}
