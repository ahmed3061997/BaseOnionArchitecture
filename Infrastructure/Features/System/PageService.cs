using Application.Common.Exceptions;
using Application.Interfaces.Persistence;
using Application.Interfaces.System;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Features.System
{
    public class PageService : IPageService
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public PageService(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task Create(PageDto page)
        {
            CheckCode(page.Code);
            CheckNames(page.Names.Select(x => x.Value).ToArray());
            context.Pages.Add(mapper.Map<Page>(page));
            await context.SaveChangesAsync();
        }

        public async Task Edit(PageDto page)
        {
            CheckCode(page.Code, page.Id);
            CheckNames(page.Names.Select(x => x.Value).ToArray(), page.Id);
            await context.UpdateAsync(mapper.Map<Page>(page));
        }

        public async Task Delete(Guid id)
        {
            //if (context.Set<PageOperation>.Any(x => x.PageId == id)) throw new DeleteUsedEntityException();
            var page = await context.Pages.FindAsync(id);
            context.Pages.Remove(page);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PageDto>> GetAll()
        {
            return await context.Pages
                   .Include(x => x.Names)
                   .AsNoTracking()
                   .Select(x => mapper.Map<PageDto>(x)).ToListAsync();
        }

        public async Task<PageDto> Get(Guid id)
        {
            var page = await context.Pages
                .Include(x => x.Names)
                .Include(x => x.Operations)
                .FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<PageDto>(page);
        }

        private void CheckCode(Pages code, Guid? exclude = null)
        {
            if (context.Pages.Any(m => m.Code == code && m.Id != exclude))
                throw new CodeUsedException();
        }

        private void CheckNames(string[] names, Guid? exclude = null)
        {
            if (context.Set<PageName>().Any(n => names.Contains(n.Name) && n.PageId != exclude))
            {
                throw new NameUsedException(context
                    .Set<PageName>()
                    .Where(n => names.Contains(n.Name))
                    .Select(x => x.Name)
                    .AsEnumerable());
            }
        }
    }
}
