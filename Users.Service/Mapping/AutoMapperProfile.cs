using AutoMapper;
using Common.Domain;
using Users.Service.Dto;
using Users.Service.Dtos;

namespace Users.Service.Mapping
{
    public class AutoMapperProfile:Profile
    {
       public AutoMapperProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<UpdateUserPasswordDto, ApplicationUser>();
            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetUserDto>();
        }
    }
}
