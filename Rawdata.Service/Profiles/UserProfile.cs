using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;

namespace Rawdata.Service.Profiles
{
    public class UserProfile : Profile
    {
        // UrlHelper is injected when the UserProfile is instantiated
        public UserProfile(IUrlHelper url)
        {
            // Add map from User to UserDto
            CreateMap<User, UserDto>()
                .ForPath(
                    dest => dest.Links.Self,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_USER_BY_ID, new { Id = src.Id }))
                );

            // Add map from UserRegisterDto to User (services should only use entity classes from the business layer)
            CreateMap<UserRegisterDto, User>();
        }
    }
}
