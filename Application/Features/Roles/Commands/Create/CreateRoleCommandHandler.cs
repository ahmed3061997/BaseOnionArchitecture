using AutoMapper;
using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using MediatR;

namespace Application.Features.Roles.Commands.Create
{
    public class EditRoleCommandHandler : IRequestHandler<CreateRoleCommand, IResponse>
    {
        private readonly IRoleService service;
        private readonly IMapper mapper;

        public EditRoleCommandHandler(IRoleService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            await service.Create(mapper.Map<RoleDto>(request));
            return new Response() { Result = true };
        }
    }
}
