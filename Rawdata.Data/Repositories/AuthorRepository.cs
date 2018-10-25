using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(DataContext context) : base(context)
        {

        }

        public virtual void Add(Author author)
        {
            Context.Set<Author>().Add(author);
        }

        public virtual async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await Context.Authors.Include(a => a.Posts).Include(a => a.Comments).ToListAsync();
        }

        public virtual void Update(Author author)
        {
            Context.Set<Author>().Update(author);
        }

        public virtual void Remove(Author author)
        {
            Context.Set<Author>().Remove(author);
        }


    }
}