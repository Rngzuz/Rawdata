using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> GetById(int id);

        IQueryable<Question> QueryQuestions(int? userId, string search, IList<string> tags, bool answeredOnly, int page, int size);

        IQueryable<Question> QueryMarkedQuestions(int userId, string search, IList<string> tags, bool answeredOnly, int page, int size);
    }
}
