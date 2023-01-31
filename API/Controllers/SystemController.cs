using API.Common;
using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Models.System;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/system")]
    [ApiController]
    public class SystemController : ControllerBase
    {

        [HttpGet(ApiRoutes.GetCultures)]
        public IEnumerable<CultureDto> GetCultures()
        {
            return Cultures.All;
        }
    }
}
