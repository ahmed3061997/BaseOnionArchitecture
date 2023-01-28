using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Responses;
using API.Common;
using Application.Features.Users.Queries.GetCurrentUser;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(ApiRoutes.GetCurrentUser)]
        public async Task<IResponse> GetCurrentUser()
        {
            return await mediator.Send(new GetCurrentUserQuery());
        }
    }
}
