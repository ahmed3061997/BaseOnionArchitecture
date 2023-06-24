using Application.Interfaces.System;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;
using Domain.Exceptions;
using Domain.Repository.Base;

namespace Application.Services.System
{
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Import(IEnumerable<PageDto> Pages)
        {
            foreach (var dto in Pages)
            {
                var Page = await _unitOfWork
                    .Pages
                    .Find(x => x.Code == dto.Code, x => x.Names);

                if (Page == null)
                {
                    Page = new Page();
                    _unitOfWork.Pages.Add(Page);
                }

                _mapper.Map(dto, Page);
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task Create(PageDto Page)
        {
            _unitOfWork.Pages.Add(_mapper.Map<Page>(Page));
            await _unitOfWork.CompleteAsync();
        }

        public async Task Edit(PageDto Page)
        {
            var entity = await _unitOfWork.Pages.Find(x => x.Id == Page.Id, x => x.Names) ?? throw new NotFoundException();
            _mapper.Map(Page, entity);
            _unitOfWork.Pages.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.Pages.Remove(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<PageDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<PageDto>>(await _unitOfWork
                .Pages
                .GetAllAsync(x => x.Names));
        }

        public async Task<PageDto?> Get(int id)
        {
            return _mapper.Map<PageDto>(await _unitOfWork
                .Pages
                .Find(x => x.Id == id, x => x.Names));
        }
    }
}
