using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController, Route("api/search"), Produces("application/json")]
    public class SearchController : BaseController
    {
        protected readonly IUserService UserService;
        protected readonly ISearchResultService SearchResultService;

        public SearchController(IMapper dtoMapper, ISearchResultService searchResultService) : base(dtoMapper)
        {
            SearchResultService = searchResultService;
        }

        [HttpGet(Name = "GetExactMatch")]
        public async Task<IActionResult> Get([FromQuery] PagingDto paging, [FromQuery] string[] words, [FromQuery] string action)
        {
            switch (action) {
                case "exact":
                    return Ok(await GetExactMatch(paging, words));
                case "best":
                    return Ok(await GetBestMatch(paging, words));
                default:
                    return Ok(await GetRankedWeightedMatch(paging, words));
            }
        }

        protected async Task<IList<SearchResult>> GetExactMatch(PagingDto paging, params string[] words)
            => await SearchResultService
                .GetExactMatch(paging.Page, paging.Size, words)
                .ToListAsync();

        protected async Task<IList<RankedSearchResult>> GetBestMatch(PagingDto paging, params string[] words)
            => await SearchResultService
                .GetBestMatch(paging.Page, paging.Size, words)
                .ToListAsync();

        protected async Task<IList<RankedSearchResult>> GetRankedWeightedMatch(PagingDto paging, params string[] words)
            => await SearchResultService
                .GetRankedWeightedMatch(paging.Page, paging.Size, words)
                .ToListAsync();

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
    }
}
