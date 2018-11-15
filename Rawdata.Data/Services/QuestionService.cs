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

        public IQueryable<Question> QueryQuestions(int? userId, string search, string[] tags, bool answeredOnly, int page, int size)
        {
            return Context.Questions
                .FromSql($"select * from query_questions({search}, {(tags.Length > 0 ? tags : null)}, {answeredOnly}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }

        public async Task<IList<Question>> GetQuestionsWithMarkedPosts(int? userId, int page, int size)
        {
            return await Context.Questions
                .FromSql($"SELECT * FROM posts_with_tags_and_marked WHERE id IN ((SELECT post_id id FROM marked_posts WHERE user_id = {userId}) UNION (SELECT t2.parent_id id FROM marked_posts t1, posts t2 WHERE t1.user_id = {userId} AND t1.post_id = t2.id))")
                .Include(q => q.PostTags)
                .Skip(size * (page - 1))
                .Take(size)
                .ToListAsync();
        }

        public async Task<IList<Question>> GetQuestionsWithMarkedComments(int? userId, int page, int size)
        {
            return await Context.Questions
                .FromSql($"SELECT t1.* FROM posts_with_tags_and_marked t1 JOIN marked_posts t2 ON t1.id = t2.post_id WHERE t2.user_id = {userId}")
                .Include(q => q.PostTags)
                .Skip(size * (page - 1))
                .Take(size)
                .ToListAsync();
        }
    }
}
