using FluentValidation.Results;

namespace Application.Interfaces.Validation
{
    public interface IValidationService
    {
        Task ThrowIfInvalid<T>(T value);
        Task<IEnumerable<ValidationFailure>> Validate<T>(T value);
    }
}
