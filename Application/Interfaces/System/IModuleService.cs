using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IModuleService
    {
        Task<ModuleDto> Get(Guid id);
        Task<IEnumerable<ModuleDto>> GetDrop();
        Task<IEnumerable<ModuleDto>> GetAll();
        Task Create(ModuleDto module);
        Task Edit(ModuleDto module);
        Task Delete(Guid id);
    }
}
