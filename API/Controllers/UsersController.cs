using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Responses;
using Application.Common.Constants;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.RefreshToken;
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
