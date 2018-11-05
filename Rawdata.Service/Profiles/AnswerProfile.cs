using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Models;
using Rawdata.Service.Models;

namespace Rawdata.Service.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile(IUrlHelper url)
        {
            CreateMap<Answer, AnswerDto>()
                .ForMember(
                    dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comments.ToList())
                )
                .ForPath(
                    dest => dest.Links.Self,
                    opt => opt.MapFrom(src => url.Link("GetQuestionById", new { src.Id }))
                )
                .ForPath(
                    dest => dest.Links.Parent,
                    opt => opt.MapFrom(src => url.Link("GetQuestionById", new { Id = src.ParentId }))
                )
                .ForPath(
                    dest => dest.Links.Author,
                    opt => opt.MapFrom(src => url.Link("GetAuthorById", new { Id = src.AuthorId }))
                );
        }
    }
}
