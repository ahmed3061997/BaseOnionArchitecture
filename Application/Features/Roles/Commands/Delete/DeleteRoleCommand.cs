using FluentValidation;
using Application.Common.Responses;
using MediatR;

namespace Application.Features.Roles.Commands.Delete
{
    public class DeleteRoleCommand : IRequest<IResponse>
    {
        public string Id { get; set; }
    }

    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
