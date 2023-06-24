using API.Common;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Contracts.Validation;
using Application.Interfaces.System;
using Application.Models.System;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/modules")]
    [ApiController]
    [Authorize(Roles = Roles.Developer)]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly IValidationService _validationService;

        public ModulesController(IModuleService moduleService, IValidationService validationService)
        {
            _moduleService = moduleService;
            _validationService = validationService;
        }

        [HttpGet(ApiRoutes.GetCodes)]
        public IEnumerable<KeyValuePair<int, string>> GetCodes()
        {
            return EnumExtensions.EnumToDictionary<Modules>();
        }

        [HttpGet(ApiRoutes.GetAll)]
        public async Task<IEnumerable<ModuleDto>> GetAll()
        {
            return await _moduleService.GetAll();
        }

        [HttpPost(ApiRoutes.Import)]
        public async Task Import(IFormFile jsonFile)
        {
            try
            {
                using var stream = new StreamReader(jsonFile.OpenReadStream());
                var json = await stream.ReadToEndAsync();
                var list = JsonConvert.DeserializeObject<IEnumerable<ModuleDto>>(json);
                await _moduleService.Import(list);
            }
            catch (Exception)
            {
                throw new ImportException();
            }
        }

        [HttpGet(ApiRoutes.Get)]
        public async Task<ModuleDto> Get(int id)
        {
            return await _moduleService.Get(id);
        }

        [HttpPost(ApiRoutes.Create)]
        public async Task<bool> Create(ModuleDto dto)
        {
            await _validationService.ThrowIfInvalid(dto);
            await _moduleService.Create(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Edit)]
        public async Task<bool> Edit(ModuleDto dto)
        {
            await _validationService.ThrowIfInvalid(dto);
            await _moduleService.Edit(dto);
            return true;
        }

        [HttpPost(ApiRoutes.Delete)]
        public async Task<bool> Delete(int id)
        {
            await _moduleService.Delete(id);
            return true;
        }
    }
}
