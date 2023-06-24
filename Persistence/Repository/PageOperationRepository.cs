using Domain.Entities.System;
using Domain.Repository;
using Persistence.Context;
using Persistence.Repository.Base;

namespace Persistence.Repository
{
    public class PageOperationRepository : GenericRepository<PageOperation>, IPageOperationRepository
    {
        public PageOperationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
