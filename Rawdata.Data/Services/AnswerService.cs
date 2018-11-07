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

<<<<<<< HEAD
        public async Task<Answer> GetById(int id)
        {
            return await Context.Answers.SingleOrDefaultAsync(q => q.Id == id);
        }

        public IQueryable<Answer> QueryAnswers(int? userId, string search, IList<string> tags, bool answeredOnly, int page, int size)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Answer> QueryMarkedAnswers(int userId, string search, IList<string> tags, bool answeredOnly, int page, int size)
        {
            throw new System.NotImplementedException();
=======
        public async Task<Answer> GetAnswerById(int id)
        {
            return await Context.Answers
                .FromSql($"select * from posts where id = {id}")
                .Include(a => a.Parent)
                .Include(a => a.Author)
                .Include(a => a.Comments)
                .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync();
>>>>>>> b509b5a123814a220766fcf4cdac4adca0731f4d
        }
    }
}
