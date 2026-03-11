using System.Runtime.CompilerServices;
using WorkerManagementApi.Application.Common.Exceptions;

namespace WorkerManagementApi.Middlewares.ExceptionHandling
{
    public class ExceptionHandlingMiddleware: IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred");

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(System.Net.HttpStatusCode.BadRequest, $"Bad request {exception.Message}"),
                KeyNotFoundException _ => new ExceptionResponse(System.Net.HttpStatusCode.NotFound, $"Not found exception {exception.Message}"),
                UnauthorizedAccessException _ => new ExceptionResponse(System.Net.HttpStatusCode.Unauthorized, $"Unauthorized {exception.Message}"),
                _ => new ExceptionResponse(System.Net.HttpStatusCode.InternalServerError, $"Internal server error {exception.Message}")
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync( response );
        }


    }
}
