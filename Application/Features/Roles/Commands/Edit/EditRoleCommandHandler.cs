using AutoMapper;
using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Roles.Commands.Edit
{
    public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, IResponse>
    {
        private readonly IRoleService service;
        private readonly IMapper mapper;

        public EditRoleCommandHandler(IRoleService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            await service.Update(mapper.Map<RoleDto>(request));
            return new Response() { Result = true };
        }
    }
}
