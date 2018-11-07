using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;

namespace Rawdata.Service.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile(IUrlHelper url)
        {
            CreateMap<Post, AnswerDto>()
                .Include<Answer, AnswerDto>();

            CreateMap<Answer, AnswerDto>()
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Author.DisplayName)
                )
                .ForMember(
                    dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comments.ToList())
                )
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_ANSWER_BY_ID, new {src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Parent,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_QUESTION_BY_ID, new { Id = src.ParentId }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new { Id = src.AuthorId }))
                );
        }
    }

    public class MarkedAnswerProfile : Profile
    {
        public MarkedAnswerProfile(IUrlHelper url)
        {
            CreateMap<MarkedPost, MarkedAnswerDto>()
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Post.Author.DisplayName)
                )
                .ForMember(
                    dest => dest.CreationDate,
                    opt => opt.MapFrom(src => src.Post.CreationDate)
                )
                .ForMember(
                    dest => dest.Score,
                    opt => opt.MapFrom(src => src.Post.Score)
                )
                .ForMember(
                    dest => dest.Body,
                    opt => opt.MapFrom(src => src.Post.Body)
                )
                // For path is used for nested member variables
                .ForPath(
                    dest => dest.Links.Self,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_ANSWER_BY_ID, new { src.Post.Id }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    // Generate absolute URL
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new { Id = src.Post.AuthorId }))
                );

            // Allow mapping to an empty collection
            AllowNullCollections = true;
        }
    }
}
