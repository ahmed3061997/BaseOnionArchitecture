using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IOperationService
    {
        Task<OperationDto> Get(int id);
        Task<IEnumerable<OperationDto>> GetAll();
        Task Import(IEnumerable<OperationDto> operations);
        Task Create(OperationDto operation);
        Task Edit(OperationDto operation);
        Task Delete(int id);
    }
}
