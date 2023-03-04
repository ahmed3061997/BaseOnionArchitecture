using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IOperationService
    {
        Task<OperationDto> Get(Guid id);
        Task<IEnumerable<OperationDto>> GetDrop();
        Task<IEnumerable<OperationDto>> GetAll();
        Task Create(OperationDto operation);
        Task Edit(OperationDto operation);
        Task Delete(Guid id);
    }
}
