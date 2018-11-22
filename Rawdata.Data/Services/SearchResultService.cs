using System.Linq;
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

        public IQueryable<RankedSearchResult> GetBestMatch(int page, int size, params string[] words)
        {
            return Context.RankedSearchResults
                .FromSql($"select * from best_match({words})")
                .OrderByDescending(m => m.Rank)
                .ThenByDescending(m => m.Post.Score)
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post);
        }

        public IQueryable<RankedSearchResult> GetRankedWeightedMatch(int page, int size, params string[] words)
        {
            return Context.RankedSearchResults
                .FromSql($"select * from ranked_weighted_match({words})")
                .OrderByDescending(m => m.Rank)
                .ThenByDescending(m => m.Post.Score)
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post);
        }

        public IQueryable<SearchResult> GetExactMatch(int page, int size, params string[] words)
        {
            return Context.SearchResults
                .FromSql($"select * from exact_match({words})")
                .OrderByDescending(m => m.Post.Score)
                .Skip(size * (page - 1))
                .Take(size)
                .Include(m => m.Post);
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
    }
}
