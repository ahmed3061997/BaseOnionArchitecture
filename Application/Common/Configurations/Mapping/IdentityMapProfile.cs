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
    public class IdentityMapProfile : Profile
    {
        public IdentityMapProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(x => x.Roles, op => op.MapFrom(x => x.Roles!.Select(y => y.Role.Name)))
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
