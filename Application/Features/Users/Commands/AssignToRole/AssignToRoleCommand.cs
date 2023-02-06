using Application.Common.Responses;
using MediatR;

namespace Application.Features.Users.Commands.AssignToRole
{
    public class AssignToRoleCommand : IRequest<IResponse>
    {
        public string UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
