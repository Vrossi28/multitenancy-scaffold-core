using System.Net.Mime;
using System.Text.Json;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore
{
    public sealed class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        public ExceptionHandler(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            var errorDetails = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = _env.IsProduction() ? "Internal Server Error" : "Exception: " + exception.Message
            };

            var result = JsonSerializer.Serialize(errorDetails);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(result);
        }
    }
}
