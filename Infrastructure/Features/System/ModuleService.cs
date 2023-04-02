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
    public class ModuleService : IModuleService
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public ModuleService(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task Import(IEnumerable<ModuleDto> modules)
        {
            foreach (var dto in modules)
            {
                var module = await context.Modules
                    .Include(x => x.Names)
                    .FirstOrDefaultAsync(x => x.Code == dto.Code);

                if (module == null)
                {
                    module = new Module();
                    context.Modules.Add(module);
                }

                mapper.Map(dto, module);
            }
            await context.SaveChangesAsync();
        }

        public async Task Create(ModuleDto module)
        {
            CheckCode(module.Code);
            CheckNames(module.Names.Select(x => x.Value).ToArray());
            context.Modules.Add(mapper.Map<Module>(module));
            await context.SaveChangesAsync();
        }

        public async Task Edit(ModuleDto module)
        {
            CheckCode(module.Code, module.Id);
            CheckNames(module.Names.Select(x => x.Value).ToArray(), module.Id);
            mapper.Map(module, await context.Modules
                .Include(x => x.Names)
                .FirstOrDefaultAsync(x => x.Id == module.Id));
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            if (context.Pages.Any(x => x.ModuleId == id)) throw new DeleteUsedEntityException();
            var module = await context.Modules.FindAsync(id);
            context.Modules.Remove(module);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ModuleDto>> GetAll()
        {
            return await context.Modules
                .Include(x => x.Names)
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .Select(x => mapper.Map<ModuleDto>(x))
                .ToListAsync();
        }

        public async Task<IEnumerable<ModuleDto>> GetDrop()
        {
            return await context.Modules
                  .Include(x => x.Names)
                  .AsNoTracking()
                  .Select(x => mapper.Map<ModuleDto>(x)).ToListAsync();
        }

        public async Task<ModuleDto> Get(Guid id)
        {
            var module = await context.Modules.Include(x => x.Names).FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<ModuleDto>(module);
        }

        private void CheckCode(Modules code, Guid? exclude = null)
        {
            if (context.Modules.Any(m => m.Code == code && m.Id != exclude))
                throw new CodeUsedException();
        }

        private void CheckNames(string[] names, Guid? exclude = null)
        {
            if (context.Set<ModuleName>().Any(n => names.Contains(n.Name) && n.ModuleId != exclude))
            {
                throw new NameUsedException(context
                    .Set<ModuleName>()
                    .Where(n => names.Contains(n.Name))
                    .Select(x => x.Name)
                    .AsEnumerable());
            }
        }
    }
}
