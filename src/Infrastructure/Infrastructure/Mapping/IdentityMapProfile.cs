using Application.Common.Constants;
using Application.Models.Common;
using Application.Models.Users;
using AutoMapper;
using Infrastructure.Mapping.Resolvers;
using Persistence.Identity;

namespace Infrastructure.Mapping
{
    public class IdentityMapProfile : Profile
    {
        public IdentityMapProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims!.Select(y => y.ClaimValue)))
                .ReverseMap()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id ?? Guid.NewGuid().ToString()))
                .ForMember(x => x.Roles, op => op.MapFrom(x => x.Roles.Select(r => new UserRole()
                {
                    UserId = x.Id,
                    RoleId = r.Id
                })))
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims.Select(x => new UserClaim()
                {
                    ClaimType = Claims.Permission,
                    ClaimValue = x
                })));

            CreateMap<UserRole, UserRoleDto>()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.RoleId))
                .ForMember(x => x.Code, op => op.MapFrom(x => x.Role.Name))
                .ForMember(x => x.Name, op => op.MapFrom<RoleNameResolver>());

            CreateMap<Role, UserRoleDto>()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id))
                .ForMember(x => x.Code, op => op.MapFrom(x => x.Name))
                .ForMember(x => x.Name, op => op.MapFrom<RoleNameResolver>());

            CreateMap<Role, RoleDto>()
                .ForMember(x => x.Code, op => op.MapFrom(x => x.Name))
                .ForMember(x => x.Name, op => op.MapFrom<RoleNameResolver>())
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims.Select(c => c.ClaimValue)))
                .ReverseMap()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id ?? Guid.NewGuid().ToString()))
                .ForMember(x => x.Name, op => op.MapFrom<NormalizedRoleNameResolver>())
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims.Select(x => new RoleClaim()
                {
                    ClaimType = Claims.Permission,
                    ClaimValue = x
                })));
            CreateMap<RoleName, CultureLookupDto>()
                .ForMember(x => x.Value, op => op.MapFrom(x => x.Name))
                .ReverseMap()
                .ForMember(x => x.Name, op => op.MapFrom(x => x.Value));
        }
    }
}
