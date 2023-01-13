using AutoMapper;
using Application.Features.Users.Commands.CreateUser;
using Application.Models.Users;
using Domain.Entities.Users;

namespace Application.Common.Configurations.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ReverseMap()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id ?? Guid.NewGuid().ToString()));
            CreateMap<CreateUserCommand, UserDto>();
            CreateMap<UserDto, CurrentUserDto>();

            CreateMap<ApplicationUserRole, RoleDto>()
                .ForMember(x => x.Claims, op => op.MapFrom(x => x.Claims.Select(c => c.ClaimValue)))
                .ReverseMap()
                .ForMember(x => x.Id, op => op.MapFrom(x => x.Id ?? Guid.NewGuid().ToString()));
        }
    }
}
