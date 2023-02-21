using Application.Interfaces.Validation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Features.Validation
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
            var errors = await Validate(value);
            if (errors.Any())
                throw new ValidationException(errors);
        }

        public async Task<IEnumerable<ValidationFailure>> Validate<T>(T value)
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
