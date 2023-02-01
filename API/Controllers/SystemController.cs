using API.Common;
using Application.Common.Constants;
using Application.Models.System;
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
