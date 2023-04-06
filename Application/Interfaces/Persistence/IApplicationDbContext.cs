﻿using Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        //DbSet<HubConnection> HubConnections { get; }
        DbSet<Module> Modules { get; }
        DbSet<Operation> Operations { get; }
        DbSet<Page> Pages { get; }
        DbSet<PageOperation> PageOperations { get; }
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
