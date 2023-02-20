using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Validation
{
    public interface IValidationService
    {
        Task ThrowIfInvalid<T>(T value);
        Task<IEnumerable<ValidationFailure>> Validate<T>(T value);
    }
}
