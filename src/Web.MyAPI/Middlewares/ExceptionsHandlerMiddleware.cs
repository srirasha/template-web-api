using FluentValidation;
using System.Net;
using System.Net.Mime;

namespace Web.MyAPI.Middlewares
{
    internal sealed class ExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionsHandlerMiddleware(RequestDelegate next, ILogger<ExceptionsHandlerMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                context.Response.StatusCode = exception switch
                {
                    ApplicationException => (int)HttpStatusCode.UnprocessableEntity,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                if (!_environment.IsProduction() || exception.GetType() == typeof(ValidationException))
                {
                    await context.Response.WriteAsJsonAsync(exception.Message);
                }

                _logger.LogError(exception, "An error occured while trying to execute a request");
            }
        }
    }
}