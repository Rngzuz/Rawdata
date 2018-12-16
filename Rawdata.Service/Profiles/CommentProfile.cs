using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;

namespace Rawdata.Service.Profiles
{
    public class CommentProfile : Profile
    {
        // UrlHelper is injected when the CommentProfile is instantiated
        public CommentProfile(IUrlHelper url)
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Author.DisplayName)
                )
                // For path is used for nested member variables
                .ForPath(
                    dest => dest.Links.Self,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_COMMENT_BY_ID, new { src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new { Id = src.AuthorId }))
                );

            // Allow mapping to an empty collection
            AllowNullCollections = true;
        }
    }

    public class MarkedCommentProfile : Profile
    {
        public MarkedCommentProfile(IUrlHelper url)
        {
            CreateMap<MarkedComment, MarkedCommentDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.CommentId)
                )
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Comment.Author.DisplayName)
                )
                .ForMember(
                    dest => dest.CreationDate, 
                    opt => opt.MapFrom(src => src.Comment.CreationDate)
                )
                .ForMember(
                    dest => dest.Score,
                    opt => opt.MapFrom(src => src.Comment.Score)
                )
                .ForMember(
                    dest => dest.Text,
                    opt => opt.MapFrom(src => src.Comment.Text)
                )
                 // For path is used for nested member variables
                .ForPath(
                    dest => dest.Links.Self,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_COMMENT_BY_ID, new { src.Comment.Id }))
                )
               .ForPath(
                    dest => dest.Links.Author,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new { Id = src.Comment.AuthorId }))
                );

            // Allow mapping to an empty collection
            AllowNullCollections = true;
        }

    }
}
