using Application.Common.Exceptions;
using Application.Interfaces.Persistence;
using Application.Interfaces.System;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.System
{
    public class OperationService : IOperationService
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public OperationService(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task Import(IEnumerable<OperationDto> operations)
        {
            foreach (var dto in operations)
            {
                var operation = await context.Operations
                    .Include(x => x.Names)
                    .FirstOrDefaultAsync(x => x.Code == dto.Code);

                if (operation == null)
                {
                    operation = new Operation();
                    context.Operations.Add(operation);
                }

                mapper.Map(dto, operation);
            }
            await context.SaveChangesAsync();
        }

        public async Task Create(OperationDto operation)
        {
            CheckCode(operation.Code);
            CheckNames(operation.Names.Select(x => x.Value).ToArray());
            context.Operations.Add(mapper.Map<Operation>(operation));
            await context.SaveChangesAsync();
        }

        public async Task Edit(OperationDto operation)
        {
            CheckCode(operation.Code, operation.Id);
            CheckNames(operation.Names.Select(x => x.Value).ToArray(), operation.Id);
            mapper.Map(operation, await context.Operations
                .Include(x => x.Names)
                .FirstOrDefaultAsync(x => x.Id == operation.Id));
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            if (context.PageOperations.Any(x => x.OperationId == id)) throw new DeleteUsedEntityException();
            var operation = await context.Operations.FindAsync(id);
            context.Operations.Remove(operation);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OperationDto>> GetAll()
        {
            return await context.Operations
                    .Include(x => x.Names)
                    .AsNoTracking()
                    .OrderBy(x => x.Code)
                    .Select(x => mapper.Map<OperationDto>(x))
                    .ToListAsync();
        }

        public async Task<IEnumerable<OperationDto>> GetDrop()
        {
            return await context.Operations.Include(x => x.Names)
                .AsNoTracking()
                .Select(x => mapper.Map<OperationDto>(x))
                .ToListAsync();
        }

        public async Task<OperationDto> Get(Guid id)
        {
            var operation = await context.Operations.Include(x => x.Names).FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<OperationDto>(operation);
        }

        private void CheckCode(Operations code, Guid? exclude = null)
        {
            if (context.Operations.Any(m => m.Code == code && m.Id != exclude))
                throw new CodeUsedException();
        }

        private void CheckNames(string[] names, Guid? exclude = null)
        {
            if (context.Set<OperationName>().Any(n => names.Contains(n.Name) && n.OperationId != exclude))
            {
                throw new NameUsedException(context
                    .Set<OperationName>()
                    .Where(n => names.Contains(n.Name))
                    .Select(x => x.Name)
                    .AsEnumerable());
            }
        }
    }
}
