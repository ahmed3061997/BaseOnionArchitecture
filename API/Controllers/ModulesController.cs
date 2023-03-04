﻿using API.Common;
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
    [Route("api/modules")]
    [ApiController]
    [Authorize(Roles = Roles.Developer)]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService moduleService;
        private readonly IValidationService validationService;

        public ModulesController(IModuleService moduleService, IValidationService validationService)
        {
            this.moduleService = moduleService;
            this.validationService = validationService;
        }

        [HttpGet(ApiRoutes.GetCodes)]
        public IEnumerable<KeyValuePair<int, string>> GetCodes()
        {
            return EnumExtensions.EnumToDictionary<Modules>();
        }

        [HttpGet(ApiRoutes.GetAll)]
        public async Task<IEnumerable<ModuleDto>> GetAll()
        {
            return await moduleService.GetAll();
        }

        [HttpGet(ApiRoutes.Get)]
        public async Task<ModuleDto> Get(Guid id)
        {
            return await moduleService.Get(id);
        }

        [HttpPost(ApiRoutes.Create)]
        public async Task<bool> Create(ModuleDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            await moduleService.Create(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Edit)]
        public async Task<bool> Edit(ModuleDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            await moduleService.Edit(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            await moduleService.Delete(id);
            return true;
        }
    }
}