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

        public async Task<Answer> GetById(int id)
        {
            return await Context.Answers
                .FromSql($"select * from posts where id = {id}")
                .Include(a => a.Parent)
                .Include(a => a.Author)
                .Include(a => a.Comments)
                .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync();
        }

        public IQueryable<Answer> QueryAnswers(int? userId, string search, int page, int size)
        {
            return Context.Answers
                .FromSql($"select * from query_answers({search}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }

        public IQueryable<Answer> QueryMarkedAnswers(int? userId, string search, int page, int size)
        {
            return Context.Answers
                .FromSql($"select * from query_marked_answers({search}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }
    }
}
