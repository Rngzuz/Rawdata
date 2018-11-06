using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class AnswerService : BaseService, IAnswerService
    {
        public AnswerService(DataContext context) : base(context)
        {
        }

        public async Task<Answer> GetAnswerById(int id)
        {
            return await Context.Answers
                .FromSql($"select * from posts where id = {id}")
                .Include(a => a.Parent)
                .Include(a => a.Author)
                .Include(a => a.Comments)
                .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync();
        }
    }
}
