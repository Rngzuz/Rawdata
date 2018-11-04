using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class CommentProfile : Profile
    {
        // UrlHelper is injected when the CommentProfile is instantiated
        public CommentProfile(IUrlHelper url)
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Author.DisplayName)
                )
                // For path is used for nested member variables
                .ForPath(
                    dest => dest.Links.Self,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link("GetCommentById", new { src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => "Author URL" /* MISSING Author URL */)
                );

            // Allow mapping to an empty collection
            AllowNullCollections = true;
        }
    }
}
