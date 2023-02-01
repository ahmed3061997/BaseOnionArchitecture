using FluentValidation;
using Application.Common.Responses;
using Application.Models.Common;
using MediatR;

namespace Application.Features.Roles.Commands.Edit
{
    public class EditRoleCommand : IRequest<IResponse>
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
    }

    public class EditRoleCommandValidator : AbstractValidator<EditRoleCommand>
    {
        public EditRoleCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Names).NotEmpty();
            RuleFor(x => x.Claims).NotEmpty();
        }
    }
}
