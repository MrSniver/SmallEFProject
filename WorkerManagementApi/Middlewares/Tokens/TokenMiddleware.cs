using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Tokens.Interfaces;

namespace WorkerManagementApi.Middlewares.Tokens
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public TokenMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, IApplicationUserRepository userRepository, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = tokenService.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userRepository.GetByIdAsync(userId.Value).Result;
            }

            await _requestDelegate(context);
        }
    }
}
