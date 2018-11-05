using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Controllers;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile(IUrlHelper url)
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(
                    dest => dest.Answers,
                    opt => opt.MapFrom(src => src.Answers.ToList())
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
}
