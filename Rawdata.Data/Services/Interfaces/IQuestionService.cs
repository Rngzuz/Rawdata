using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> GetQuestionById(int id);
        IQueryable<Question> QueryQuestions(int? userId, string search, string[] tags, bool answeredOnly, int page, int size);
        Task<IList<Question>> GetQuestionsWithMarkedPosts(int? userId, int page, int size);
        Task<IList<Question>> GetQuestionsWithMarkedComments(int? userId, int page, int size);
        Task<IList<MatchResult>> GetExactMatch(params string[] words);
    }
}
