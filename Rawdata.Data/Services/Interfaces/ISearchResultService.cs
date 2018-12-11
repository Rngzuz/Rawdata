using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface ISearchResultService
    {
        Task<PaginatedResult<SearchResult>> GetExactMatchAsync(int page, int size, params string[] words);
        IQueryable<SearchResult> GetBestMatch(int page, int size, params string[] words);
        IQueryable<SearchResult> GetRankedWeightedMatch(int page, int size, params string[] words);
        IQueryable<WeightedKeyword> GetWeightedKeywords(int size, string word);
        IQueryable<WordAssociation> GetWordAssociation(int size, string word);
    }
}
