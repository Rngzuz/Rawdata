using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class QuestionService : BaseService, IQuestionService
    {
        public QuestionService(DataContext context) : base(context)
        {
        }

        public async Task<Question> GetQuestionById(int id)
        {
            return await Context.Questions
                .FromSql($"select * from posts_with_tags where id = {id}")
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Author)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Comments)
                .Include(q => q.Comments)
                    .ThenInclude(c => c.Author)
                .Include(q => q.Author)
                .Include(q => q.PostTags)
                .FirstOrDefaultAsync();
        }
        
    }
}
