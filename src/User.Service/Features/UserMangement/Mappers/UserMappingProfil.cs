using common.Entities;
using User.Service.Entities;
using User.Service.Features.UserMangement.Dtos;

namespace User.Service.Features.UserMangement.Mappers
{
    public class UserMappingProfile : AutoMapper.Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ReverseMap();

            CreateMap<UserSignUpDto, ApplicationUser>()
               .ReverseMap();

            CreateMap<ApplicationUser, UserResponseDto>()
               .ReverseMap();

        }
    }
}
