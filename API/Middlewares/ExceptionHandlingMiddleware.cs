using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;
using Application.Common.Responses;
using Localization;
using Application.Common.Exceptions;

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
        private readonly IStringLocalizer<ExceptionResource> localizer;

        public ExceptionHandlingMiddleware(IStringLocalizer<ExceptionResource> localizer)
        {
            this.localizer = localizer;
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
            var response = new Response()
            {
                Result = false,
                Errors = LocalizeErrors(GetErrors(exception))
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
            return errors.Select(x => localizer[x].ToString());
        }

        private IEnumerable<string> GetErrors(Exception exception)
        {
            if (exception is ValidationException validationException)
                return validationException.Errors.Select(x => x.ErrorMessage);

            else if (exception is AuthenticationException authenticationException)
                return GetAuthenticationError(authenticationException);

            else if (exception is IdentityException identityException)
                return identityException.Result.Errors.Select(x => x.Description);

            return new[] { exception.Message };
        }

        private IEnumerable<string> GetAuthenticationError(AuthenticationException authenticationException)
        {
            var errors = new List<string>();
            if (authenticationException.Result.IsNotAllowed)
                errors.Add("Your account is blocked");

            else if (authenticationException.Result.IsLockedOut)
                errors.Add("Your account is locked");

            else
                errors.Add("Invalid Username or Password");
            return errors;
        }
    }
}
