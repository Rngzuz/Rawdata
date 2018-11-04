using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile(IUrlHelper url)
        {
            CreateMap<User, UserDto>()
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link("GetUserById", new { Id = src.Id }))
                );

            CreateMap<UserRegisterDto, User>();

            AllowNullCollections = true;
        }
    }
}
