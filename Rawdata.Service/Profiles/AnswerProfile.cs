using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;
using Rawdata.Service.Controllers;
using System;

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
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(
                    dest => dest.QuestionId,
                    opt => opt.MapFrom(src => src.ParentId)
                )
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
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_ANSWER_BY_ID, new {src.Id}))
                )
                .ForPath(
                    dest => dest.Links.Parent,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_QUESTION_BY_ID, new {Id = src.ParentId}))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    opt => opt.MapFrom(src => url.Link(BaseController.GET_AUTHOR_BY_ID, new {Id = src.AuthorId}))
                );
        }
    }
}
