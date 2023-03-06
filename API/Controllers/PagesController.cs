using API.Common;
using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Interfaces.System;
using Application.Interfaces.Validation;
using Application.Models.System;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/pages")]
    [ApiController]
    [Authorize(Roles = Roles.Developer)]
    public class PagesController : ControllerBase
    {
        private readonly IPageService pageService;
        private readonly IValidationService validationService;

        public PagesController(IPageService pageService, IValidationService validationService)
        {
            this.pageService = pageService;
            this.validationService = validationService;
        }

        [HttpGet(ApiRoutes.GetCodes)]
        public IEnumerable<KeyValuePair<int, string>> GetCodes()
        {
            return EnumExtensions.EnumToDictionary<Pages>();
        }

        [HttpGet(ApiRoutes.GetAll)]
        public async Task<IEnumerable<PageDto>> GetAll()
        {
            return await pageService.GetAll();
        }

        [HttpGet(ApiRoutes.Get)]
        public async Task<PageDto> Get(Guid id)
        {
            return await pageService.Get(id);
        }

        [HttpPost(ApiRoutes.Create)]
        public async Task<bool> Create(PageDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            await pageService.Create(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Edit)]
        public async Task<bool> Edit(PageDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            await pageService.Edit(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            await pageService.Delete(id);
            return true;
        }
    }
}
