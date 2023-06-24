using Application.Common.Exceptions;
using Application.Interfaces.System;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;
using Domain.Repository.Base;

namespace Application.Services.System
{
    public class OperationService : IOperationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OperationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Import(IEnumerable<OperationDto> Operations)
        {
            foreach (var dto in Operations)
            {
                var Operation = await _unitOfWork
                    .Operations
                    .Find(x => x.Code == dto.Code, x => x.Names);

                if (Operation == null)
                {
                    Operation = new Operation();
                    _unitOfWork.Operations.Add(Operation);
                }

                _mapper.Map(dto, Operation);
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task Create(OperationDto Operation)
        {
            _unitOfWork.Operations.Add(_mapper.Map<Operation>(Operation));
            await _unitOfWork.CompleteAsync();
        }

        public async Task Edit(OperationDto Operation)
        {
            var entity = await _unitOfWork.Operations.Find(x => x.Id == Operation.Id, x => x.Names) ?? throw new NotFoundException();
            _mapper.Map(Operation, entity);
            _unitOfWork.Operations.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.Operations.Remove(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<OperationDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<OperationDto>>(await _unitOfWork
                .Operations
                .GetAllAsync(x => x.Names));
        }

        public async Task<OperationDto?> Get(int id)
        {
            return _mapper.Map<OperationDto>(await _unitOfWork
                .Operations
                .Find(x => x.Id == id, x => x.Names));
        }
    }
}
