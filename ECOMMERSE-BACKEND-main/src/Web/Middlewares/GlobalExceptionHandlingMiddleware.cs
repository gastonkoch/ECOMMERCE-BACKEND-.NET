using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Ecommerce.Web.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AppValidationException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "Validation Error",
                    Title = "A validation error occurred",
                    Detail = ex.Message
                };

                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "Resource Not Found",
                    Title = "Resource not found",
                    Detail = ex.Message
                };

                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (NotAllowedException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Forbidden,
                    Type = "Forbidden",
                    Title = "Access Denied",
                    Detail = ex.Message
                };

                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = "An internal server error occurred."
                };

                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
