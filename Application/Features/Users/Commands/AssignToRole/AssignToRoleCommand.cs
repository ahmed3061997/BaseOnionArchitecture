using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Responses;

namespace Application.Features.Users.Commands.AssignToRole
{
    public class AssignToRoleCommand : IRequest<IResponse>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
