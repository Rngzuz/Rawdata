using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile(IUrlHelper url)
        {
            CreateMap<Author, AuthorDto>()
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link("GetAuthorById", new { Id = src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Questions,
                    opt => opt.MapFrom(src => url.Link("QueryQuestions", new { }))
                )
                .ForPath(
                    dest => dest.Links.Comments,
                    opt => opt.MapFrom(src => url.Link("QueryComments", new { }))
                );
        }
    }
}
