using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories
{
    public class SearchRepository : Repository<Search>, ISearchRepository
    {
        public SearchRepository(DataContext context) : base(context)
        {
        }

        public virtual Task<Search> GetById(int id)
        {
            return Context.Searches.SingleOrDefaultAsync(a => a.Id == id);
        }

        public virtual void Add(Search entity)
        {
            Context.Set<Search>().Add(entity);
        }

        public virtual async Task<IEnumerable<Search>> GetAllAsync()
        {
            return await Context.Searches.ToListAsync();
        }

        public virtual void Remove(Search entity)
        {
            Context.Set<Search>().Remove(entity);
        }


        public virtual void Update(Search entity)
        {
            Context.Set<Search>().Update(entity);
        }
    }
}
