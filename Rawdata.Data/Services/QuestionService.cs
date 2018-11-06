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
                .Include(q => q.Comments)
                    .ThenInclude(c => c.Author)
                .Include(q => q.Author)
                .Include(q => q.PostTags)
                .FirstAsync();
        }

        public IQueryable<Question> QueryQuestions(int? userId, string search, string[] tags, bool answeredOnly, int page, int size)
        {
            return Context.Questions
                .FromSql($"select * from query_posts({search}, {(tags.Length > 0 ? tags : null)}, {answeredOnly}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }

        public IQueryable<Question> QueryMarkedQuestions(int? userId, string search, string[] tags, bool answeredOnly, int page, int size)
        {
            return Context.Questions
                .FromSql($"select * from query_marked_posts({search}, {(tags.Length > 0 ? tags : null)}, {answeredOnly}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }
    }
}
