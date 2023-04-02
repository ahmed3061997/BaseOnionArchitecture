using AutoMapper;
using Application.Models.Users;
using Domain.Entities.Users;
using Application.Common.Constants;
using Application.Models.Common;
using Application.Models.System;
using Domain.Entities.System;
using Application.Common.Configurations.Mapping.Resolvers;

namespace Application.Common.Configurations.Mapping
{
    public class SystemMapProfile : Profile
    {
        public SystemMapProfile()
        {
            CreateMap<Module, ModuleDto>()
                .ForMember(x => x.Name, op => op.MapFrom<ModuleNameResolver>())
                .ReverseMap();
            CreateMap<ModuleName, CultureLookupDto>()
                .ForMember(x => x.Value, op => op.MapFrom(x => x.Name))
                .ReverseMap()
                .ForMember(x => x.Name, op => op.MapFrom(x => x.Value));

            CreateMap<Operation, OperationDto>()
                .ForMember(x => x.Name, op => op.MapFrom<OperationNameResolver>())
                .ReverseMap();
            CreateMap<OperationName, CultureLookupDto>()
                .ForMember(x => x.Value, op => op.MapFrom(x => x.Name))
                .ReverseMap()
                .ForMember(x => x.Name, op => op.MapFrom(x => x.Value));

            CreateMap<Page, PageDto>()
                .ForMember(x => x.Name, op => op.MapFrom<PageNameResolver>())
                .ReverseMap();
            CreateMap<PageOperation, PageOperationDto>()
                .ReverseMap();
            CreateMap<PageName, CultureLookupDto>()
                .ForMember(x => x.Value, op => op.MapFrom(x => x.Name))
                .ReverseMap()
                .ForMember(x => x.Name, op => op.MapFrom(x => x.Value));
        }
    }
}
