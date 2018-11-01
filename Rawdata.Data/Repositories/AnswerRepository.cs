using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class AnswerRepository : RepositoryBase, IAnswerRepository
    {
        public AnswerRepository(DataContext context) : base(context)
        {
        }

        public virtual Task<Answer> GetById(int id)
        {
            return Context.Answers
                .Include(a => a.Author)
                .Include(a => a.Parent)
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await Context.Answers
                .Include(a => a.Author)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.Author)
                .ToListAsync();
        }
    }
}
