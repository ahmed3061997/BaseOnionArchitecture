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
    [Route("api/operations")]
    [ApiController]
    [Authorize(Roles = Roles.Developer)]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationService operationService;
        private readonly IValidationService validationService;

        public OperationsController(IOperationService operationService, IValidationService validationService)
        {
            this.operationService = operationService;
            this.validationService = validationService;
        }

        [HttpGet(ApiRoutes.GetCodes)]
        public IEnumerable<KeyValuePair<int, string>> GetCodes()
        {
            return EnumExtensions.EnumToDictionary<Operations>();
        }

        [HttpGet(ApiRoutes.GetAll)]
        public async Task<IEnumerable<OperationDto>> GetAll()
        {
            return await operationService.GetAll();
        }

        [HttpGet(ApiRoutes.Get)]
        public async Task<OperationDto> Get(Guid id)
        {
            return await operationService.Get(id);
        }

        [HttpPost(ApiRoutes.Create)]
        public async Task<bool> Create(OperationDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            await operationService.Create(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Edit)]
        public async Task<bool> Edit(OperationDto dto)
        {
            await validationService.ThrowIfInvalid(dto);
            await operationService.Edit(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            await operationService.Delete(id);
            return true;
        }
    }
}