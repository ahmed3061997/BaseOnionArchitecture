using FluentValidation;
using Infrastructure.Common.Exceptions;

namespace Infrastructure.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<string> GetErrors(this Exception exception)
        {
            if (exception is ValidationException validationException)
                return validationException.Errors.Select(x => x.ErrorMessage);

            else if (exception is AuthenticationException authenticationException)
                return GetAuthenticationError(authenticationException);

            else if (exception is IdentityException identityException)
                return identityException.Result.Errors.Select(x => x.Description);

            return new[] { exception.Message };
        }

        private static IEnumerable<string> GetAuthenticationError(AuthenticationException authenticationException)
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
