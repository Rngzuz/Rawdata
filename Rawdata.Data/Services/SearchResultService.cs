using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;


namespace Rawdata.Data.Services
{
    public class SearchResultService : BaseService, ISearchResultService
    {
        public SearchResultService(DataContext context) : base(context)
        {
        }

        public async Task<PaginatedResult<SearchResult>> GetBestMatch(int page, int size, params string[] words)
        {
            var results = await Context.SearchResults
                .FromSql($"select * from best_match({words})")
                .OrderByDescending(m => m.Rank)
                .ThenByDescending(m => m.Post.Score)
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post)
                    .ThenInclude(p => p.Author)
                .ToListAsync();

            var tCount = totalCount(words);
            
            return new PaginatedResult<SearchResult>
            {
                Items = results,
                CurrentPage = page,
                PageCount = CalculatePages(tCount.Result, size)
            };

        }

        public async Task<PaginatedResult<SearchResult>> GetRankedWeightedMatch(int page, int size, params string[] words)
        {
            var results = await Context.SearchResults
                .FromSql($"select * from ranked_weighted_match({words})")
                .OrderByDescending(m => m.Rank)
                .ThenByDescending(m => m.Post.Score)
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post)
                    .ThenInclude(p => p.Author)
                .ToListAsync();

            var tCount = totalCount(words);

            return new PaginatedResult<SearchResult>
            {
                Items = results,
                CurrentPage = page,
                PageCount = CalculatePages(tCount.Result, size)
            };
    
        }

        public async Task<PaginatedResult<SearchResult>> GetExactMatchAsync(int page, int size, params string[] words)
        {
           
            var results = await Context.SearchResults
                .FromSql($"select * from exact_match({words})")
                .OrderByDescending(m => m.Post.Score)
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post)
                    .ThenInclude(p => p.Author)
                .ToListAsync();
            var tCount = totalCount(words);

            return new PaginatedResult<SearchResult>
            {
                Items = results,
                CurrentPage = page,
                PageCount = CalculatePages(tCount.Result, size)
            };

        }

        public IQueryable<WeightedKeyword> GetWeightedKeywords(int size, string word)
        {
            return Context.WeightedKeywords
                .FromSql($"select * from word_to_word({word})")
                .Take(size);
        }

        public IQueryable<WordAssociation> GetWordAssociation(int size, string word)
        {
            return Context.WordAssociations
                .FromSql($"select * from get_word_association({word})")
                .Take(size);
        }

        private async Task<int> totalCount(params string[] words)
        {
            var query = Context.SearchResults
                .FromSql($"select * from exact_match({words})")
                .OrderByDescending(m => m.Post.Score);

            return await query.CountAsync();
        }
        private int CalculatePages(int totalCount, int size)
        {
            int remainder = totalCount % size;
            int pageCount = totalCount / size;

            if (remainder > 0)
            {
                pageCount++;
            }

            return pageCount;
        }
    }
}
