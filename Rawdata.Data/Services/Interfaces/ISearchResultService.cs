using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface ISearchResultService
    {
        IQueryable<SearchResult> GetExactMatch(int page, int size, params string[] words);
        IQueryable<RankedSearchResult> GetBestMatch(int page, int size, params string[] words);
        IQueryable<RankedSearchResult> GetRankedWeightedMatch(int page, int size, params string[] words);
    }
}
