using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class AuthorService : BaseService, IAuthorService
    {
        public AuthorService(DataContext context) : base(context)
        {

        }

        public virtual Task<Author> GetById(int id)
        {
            return Context.Authors.SingleOrDefaultAsync(a => a.Id == id);
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
