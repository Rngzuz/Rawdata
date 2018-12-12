using System;
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
            var query = Context.SearchResults
                .FromSql($"select * from best_match_context({words})")
                .OrderByDescending(m => m.Rank)
                .ThenByDescending(m => m.Post.Score);

            var totalCount = await query.CountAsync();

            var results = await query
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post)
                .ThenInclude(p => p.Author)
                .ToListAsync();
            
            return new PaginatedResult<SearchResult>
            {
                Items = results,
                CurrentPage = page,
                PageCount = CalculatePages(totalCount, size)
            };

        }

        public async Task<PaginatedResult<SearchResult>> GetRankedWeightedMatch(int page, int size, params string[] words)
        {
            var query = Context.SearchResults
                .FromSql($"select * from ranked_weighted_match_context({words})")
                .OrderByDescending(m => m.Rank)
                .ThenByDescending(m => m.Post.Score);

            var totalCount = await query.CountAsync();

            var results = await query
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post)
                .ThenInclude(p => p.Author)
                .ToListAsync();

            return new PaginatedResult<SearchResult>
            {
                Items = results,
                CurrentPage = page,
                PageCount = CalculatePages(totalCount, size)
            };
    
        }

        public async Task<PaginatedResult<SearchResult>> GetExactMatchAsync(int page, int size, params string[] words)
        {
            var query = Context.SearchResults
                .FromSql($"select * from exact_match_context({words})")
                .OrderByDescending(m => m.Post.Score);

            var totalCount = await query.CountAsync();

            var results = await query
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post)
                    .ThenInclude(p => p.Author)
                .ToListAsync();
           
            return new PaginatedResult<SearchResult>
            {
                Items = results,
                CurrentPage = page,
                PageCount = CalculatePages(totalCount, size)
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

        public async Task<ForceGraphInput> GetForceGraphInput(string word, int grade)
        {
            return await Context
                .ForceGraphInputs
                .FromSql($"select STRING_AGG(line, ' ') as input from generate_force_graph_input({word}, {grade}) ").SingleOrDefaultAsync();

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
