using FluentValidation;
using Newtonsoft.Json;
using System.Net;

namespace LoanServiceAPI.Middleware
{
    internal sealed class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = MapExceptionToStatusCode(exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Error = exception.Message,
            };

            var json = JsonConvert.SerializeObject(response, Formatting.Indented);

            return context.Response.WriteAsync(json);
        }

        private static int MapExceptionToStatusCode(Exception ex) => ex switch
        {
            ValidationException _ => (int)HttpStatusCode.BadRequest,
            ArgumentException _ => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException _ => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException _ => (int)HttpStatusCode.Unauthorized,
            InvalidOperationException _ => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}