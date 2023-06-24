using Application.Common.Exceptions;
using Application.Interfaces.System;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;
using Domain.Repository.Base;

namespace Application.Services.System
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Import(IEnumerable<ModuleDto> modules)
        {
            foreach (var dto in modules)
            {
                var module = await _unitOfWork
                    .Modules
                    .Find(x => x.Code == dto.Code, x => x.Names);

                if (module == null)
                {
                    module = new Module();
                    _unitOfWork.Modules.Add(module);
                }

                _mapper.Map(dto, module);
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task Create(ModuleDto module)
        {
            _unitOfWork.Modules.Add(_mapper.Map<Module>(module));
            await _unitOfWork.CompleteAsync();
        }

        public async Task Edit(ModuleDto module)
        {
            var entity = await _unitOfWork.Modules.Find(x => x.Id == module.Id, x => x.Names) ?? throw new NotFoundException();
            _mapper.Map(module, entity);
            _unitOfWork.Modules.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.Modules.Remove(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ModuleDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<ModuleDto>>(await _unitOfWork
                .Modules
                .GetAllAsync(x => x.Names));
        }

        public async Task<ModuleDto?> Get(int id)
        {
            return _mapper.Map<ModuleDto>(await _unitOfWork
                .Modules
                .Find(x => x.Id == id, x => x.Names));
        }
    }
}
