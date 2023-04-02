﻿using Application.Models.System;

namespace Application.Interfaces.System
{
    public interface IPageService
    {
        Task<PageDto> Get(Guid id);
        Task<IEnumerable<PageDto>> GetAll();
        Task Import(IEnumerable<PageDto> pages);
        Task Create(PageDto page);
        Task Edit(PageDto page);
        Task Delete(Guid id);
    }
}
