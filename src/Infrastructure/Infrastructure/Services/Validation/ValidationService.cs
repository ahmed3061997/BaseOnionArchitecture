using Application.Contracts.Validation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Validation
{
    public class ValidationService : IValidationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ValidationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task ThrowIfInvalid<T>(T value)
        {
            var errors = await GetErrors(value);
            if (errors.Any())
                throw new ValidationException(errors);
        }

        public async Task<IEnumerable<string>> Validate<T>(T value)
        {
            var errors = await GetErrors(value);
            return errors.Select(x => x.ErrorMessage);
        }

        private async Task<IEnumerable<ValidationFailure>> GetErrors<T>(T value)
        {
            var validators = httpContextAccessor.HttpContext.RequestServices.GetServices<IValidator<T>>();
            if (validators!.Any())
            {
                var context = new ValidationContext<T>(value);

                var validationResults = await Task.WhenAll(
                validators.Select(v =>
                        v.ValidateAsync(context)));

                return validationResults
                       .Where(r => r.Errors.Any())
                       .SelectMany(r => r.Errors);
            }
            return Enumerable.Empty<ValidationFailure>();
        }
    }
}
