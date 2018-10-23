using System.Collections;
using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories
{
    public interface ISearchRepository : IRepository<Search>
    {
        void AddToSearchHistory(int userId, string searchText);

        IEnumerable<Search> GetUserSearchHistory(int userId);


    }
}