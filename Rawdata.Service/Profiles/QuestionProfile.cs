using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Data.Models.Relationships;
using Rawdata.Service.Controllers;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile(IUrlHelper url)
        {
            CreateMap<PostTag, string>()
                .ConvertUsing(source => source.TagName);


            CreateMap<Question, QuestionListDto>()
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Author.DisplayName)
                )
                .ForMember(
                    dest => dest.Tags,
                    opt => opt.MapFrom(src => src.PostTags.ToList())
                )
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_QUESTION_BY_ID, new { src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new { Id = src.AuthorId }))
                );

            CreateMap<Question, QuestionDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(
                    dest => dest.AuthorDisplayName,
                    opt => opt.MapFrom(src => src.Author.DisplayName)
                )
                .ForMember(
                    dest => dest.Answers,
                    opt => opt.MapFrom(src => src.Answers.ToList())
                )
                .ForMember(
                    dest => dest.Tags,
                    opt => opt.MapFrom(src => src.PostTags.ToList())
                )
                .ForMember(
                    dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comments.ToList())
                )
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_QUESTION_BY_ID, new { src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new { Id = src.AuthorId }))
                );
        }
    }

    public class MarkedQuestionProfile : Profile
    {
        public MarkedQuestionProfile(IUrlHelper url)
        {
            CreateMap<MarkedPost, MarkedQuestionDto>()
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
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_QUESTION_BY_ID, new { src.Post.Id }))
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
