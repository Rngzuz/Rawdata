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
            Context.Authors.Include()
            return await Context.Set<T>().ToListAsync();
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