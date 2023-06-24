namespace Application.Contracts.Validation
{
    public interface IValidationService
    {
        Task ThrowIfInvalid<T>(T value);
        Task<IEnumerable<string>> Validate<T>(T value);
    }
}
