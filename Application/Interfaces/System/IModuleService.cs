using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IModuleService
    {
        Task<ModuleDto> Get(Guid id);
        Task<IEnumerable<ModuleDto>> GetDrop();
        Task<IEnumerable<ModuleDto>> GetAll();
        Task Import(IEnumerable<ModuleDto> modules);
        Task Create(ModuleDto module);
        Task Edit(ModuleDto module);
        Task Delete(Guid id);
    }
}
