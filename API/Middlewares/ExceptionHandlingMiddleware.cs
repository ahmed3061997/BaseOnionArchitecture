using Infrastructure.Common.Extensions;
using Infrastructure.Localization;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }

    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IStringLocalizer<ExceptionResource> _localizer;

        public ExceptionHandlingMiddleware(IStringLocalizer<ExceptionResource> localizer)
        {
         _localizer = localizer;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UnauthorizedAccessException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            var response = new
            {
                Result = false,
                Errors = LocalizeErrors(exception.GetErrors())
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                }));
        }

        private IEnumerable<string> LocalizeErrors(IEnumerable<string> errors)
        {
            return errors.Select(x => _localizer[x].ToString());
        }
    }
}
