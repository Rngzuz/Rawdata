using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class DeactivatedSearchRespository : Repository<DeactivatedSearch>, IDeactivatedSearchRepository
    {
        public DeactivatedSearchRespository(DataContext context) : base(context)
        {
        }

        public virtual Task<DeactivatedSearch> GetById(int id)
        {
            return Context.DeactivatedSearches.SingleOrDefaultAsync(a => a.Id == id);
        }

        public void Add(DeactivatedSearch entity)
        {
            Context.Set<DeactivatedSearch>().Add(entity);
        }

        public virtual async Task<IEnumerable<DeactivatedSearch>> GetAllAsync()
        {
            return await Context.DeactivatedSearches.ToListAsync();
        }

        public void Update(DeactivatedSearch entity)
        {
            Context.Set<DeactivatedSearch>().Update(entity);
        }

        public void Remove(DeactivatedSearch entity)
        {
            Context.Set<DeactivatedSearch>().Remove(entity);
        }

        
    }
}
