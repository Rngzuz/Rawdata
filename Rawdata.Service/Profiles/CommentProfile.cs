using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile(IUrlHelper url)
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Author.DisplayName)
                )
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link("GetCommentById", new { src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    opt => opt.MapFrom(src => "Author URL" /* MISSING Author URL */)
                );


            AllowNullCollections = true;
        }
    }
}
