using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DataContext context) : base(context)
        {

        }

        public virtual Task<Comment> GetById(int id)
        {
            return Context.Comments.SingleOrDefaultAsync(a => a.Id == id);
        }

        public virtual void Add(Comment comment)
        {
            Context.Set<Comment>().Add(comment);
        }

        public virtual async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await Context.Comments.ToListAsync();
        }

        public virtual void Update(Comment comment)
        {
            Context.Set<Comment>().Update(comment);
        }

        public virtual void Remove(Comment comment)
        {
            Context.Set<Comment>().Remove(comment);
        }
    }
}