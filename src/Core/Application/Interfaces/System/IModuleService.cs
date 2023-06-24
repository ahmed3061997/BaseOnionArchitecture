using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IModuleService
    {
        Task<ModuleDto?> Get(int id);
        Task<IEnumerable<ModuleDto>> GetAll();
        Task Import(IEnumerable<ModuleDto> modules);
        Task Create(ModuleDto module);
        Task Edit(ModuleDto module);
        Task Delete(int id);
    }
}
