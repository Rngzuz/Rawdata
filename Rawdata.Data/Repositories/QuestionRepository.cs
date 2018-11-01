using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class QuestionRepository : RepositoryBase, IQuestionRepository
    {
        public QuestionRepository(DataContext context) : base(context)
        {
        }


        public Task<Question> GetById(int entityId)
        {
            return Context.Questions
                .Include(q => q.Author)
                .Include(q => q.Answers)
                .ThenInclude(a => a.Author)
                .Include(q => q.Comments)
                .ThenInclude(c => c.Author)
                .SingleOrDefaultAsync(q => q.Id == entityId);
        }

        public virtual async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await Context.Questions
                .Include(q => q.Author)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Author)
                .Include(q => q.Comments)
                    .ThenInclude(c => c.Author)
                .ToListAsync();
;        }

        public void Add(Question question)
        {
            Context.Questions.Add(question);
        }

        public void Update(Question question)
        {
            Context.Questions.Update(question);
        }

        public void Remove(Question question)
        {
            Context.Questions.Remove(question);
        }

        public IEnumerable<Question> FilterByTags(IList<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Question> FilterByWords(int userId, string searchString, IList<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Question> GetLinkedPosts(int postId)
        {
            throw new System.NotImplementedException();
        }
    }
}
