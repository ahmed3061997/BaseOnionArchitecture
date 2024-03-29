﻿using Application.Contracts.Culture;
using Application.Models.System;
using AutoMapper;
using Domain.Entities.System;

namespace Application.Mapping.Resolvers
{
    public class PageNameResolver : IValueResolver<Page, PageDto, string>
    {
        private readonly ICurrentCultureService currentCultureService;

        public PageNameResolver(ICurrentCultureService currentCultureService)
        {
            this.currentCultureService = currentCultureService;
        }

        public string Resolve(Page source, PageDto destination, string destMember, ResolutionContext context)
        {
            var culture = currentCultureService.GetCurrentUICulture();
            return source.Names?.Where(x => x.Culture == culture).Select(x => x.Name).FirstOrDefault();
        }
    }
}
