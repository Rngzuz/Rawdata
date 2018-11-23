using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController, Route("api/search"), Produces("application/json")]
    public class SearchController : BaseController
    {
        protected readonly ISearchResultService SearchResultService;

        public SearchController(IMapper dtoMapper, ISearchResultService searchResultService) : base(dtoMapper)
        {
            SearchResultService = searchResultService;
        }

        [HttpGet("exact", Name = "GetExactMatch")]
        public async Task<IActionResult> GetExactMatch([FromQuery]Paging paging)
        {
            var result = await SearchResultService
               .GetExactMatch(paging.Page, paging.Size, paging.Words)
               .ToListAsync();

            //TODO: We want to find a better approach to map
            var items = new List<dynamic>();

            foreach (var item in result) {
                dynamic obj = new ExpandoObject();

                if (item.Post is Question) {
                    obj.Post = DtoMapper.Map<QuestionDto>(item.Post);
                }
                else {
                    obj.Post = DtoMapper.Map<AnswerDto>(item.Post);
                }

                items.Add(obj);
            }

            return Ok(items);
        }

        [HttpGet("best", Name = "GetBestMatch")]
        public async Task<IActionResult> GetBestMatch([FromQuery]Paging paging)
        {
            var result = await SearchResultService
                .GetBestMatch(paging.Page, paging.Size, paging.Words)
                .ToListAsync();

            return Ok(DirtyMap(result));
        }

        [HttpGet("ranked", Name = "GetRankedWeightedMatch")]
        public async Task<IActionResult> GetRankedWeightedMatch([FromQuery]Paging paging)
        {
            var result = await SearchResultService
                .GetRankedWeightedMatch(paging.Page, paging.Size, paging.Words)
                .ToListAsync();

            return Ok(DirtyMap(result));
        }

        [HttpGet("words", Name = "GetWords")]
        public async Task<IActionResult> GetWords([FromQuery] string word, [FromQuery] int size = 100)
        {
            var result = await SearchResultService
                .GetWeightedKeywords(size, word)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("context", Name = "GetContext")]
        public async Task<IActionResult> GetContext([FromQuery] string word, [FromQuery] int size = 100)
        {
            var result = await SearchResultService
                .GetWordAssociation(size, word)
                .ToListAsync();

            return Ok(result);
        }

        //TODO: We want to find a better approach to map
        protected IList<dynamic> DirtyMap(IList<RankedSearchResult> result)
        {
            var items = new List<dynamic>();

            foreach (var item in result) {
                dynamic obj = new ExpandoObject();
                obj.Rank = item.Rank;

                if (item.Post is Question) {
                    obj.Post = DtoMapper.Map<QuestionDto>(item.Post);
                }
                else {
                    obj.Post = DtoMapper.Map<AnswerDto>(item.Post);
                }

                items.Add(obj);
            }

            return items;
        }
    }
}
