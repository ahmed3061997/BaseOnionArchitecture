using AutoMapper;
using Application.Common.Responses;
using Application.Interfaces.Users;
using Application.Models.Users;
using Domain.Events;
using MediatR;

namespace Application.Features.Users.Commands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResultResponse<AuthResult>>
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IPublisher publisher;

        public CreateUserCommandHandler(
            IMapper mapper,
            IUserService userService,
            IPublisher publisher)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.publisher = publisher;
        }

        public async Task<IResultResponse<AuthResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<UserDto>(request);
            var result = await userService.Create(user, request.Password);
            await publisher.Publish(new UserCreatedEvent(result.User.Id, result.User.Email, request.Role, request.ConfirmEmailUrl));
            return new ResultResponse<AuthResult>() { Result = true, Value = result };
        }
    }
}
