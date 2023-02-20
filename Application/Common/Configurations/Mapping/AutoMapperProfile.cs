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
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Identity
            MapIdentityModels();
            //System
            MapSystemModels();
        }

        private void MapSystemModels()
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

        private void MapIdentityModels()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ReverseMap()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id ?? Guid.NewGuid().ToString()));

            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims.Select(c => c.ClaimValue)))
                .ForMember(x => x.Name, op => op.MapFrom<RoleNameResolver>())
                .ReverseMap()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id ?? Guid.NewGuid().ToString()))
                .ForMember(x => x.Name, op => op.MapFrom<NormalizedRoleNameResolver>())
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims.Select(x => new ApplicationRoleClaim()
                {
                    ClaimType = Claims.Permission,
                    ClaimValue = x
                })));
            CreateMap<ApplicationRoleName, CultureLookupDto>()
                .ForMember(x => x.Value, op => op.MapFrom(x => x.Name))
                .ReverseMap()
                .ForMember(x => x.Name, op => op.MapFrom(x => x.Value));
        }
    }
}
