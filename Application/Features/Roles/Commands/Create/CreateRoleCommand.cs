using FluentValidation;
using Application.Common.Responses;
using Application.Models.Common;
using MediatR;

namespace Application.Features.Roles.Commands.Create
{
    public class CreateRoleCommand : IRequest<IResponse>
    {
        public bool IsActive { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
    }

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Names).NotEmpty();
            RuleFor(x => x.Claims).NotEmpty();
        }
    }
}
