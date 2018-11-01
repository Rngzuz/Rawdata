using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class QuestionService : ServiceBase, IQuestionService
    {
        public QuestionService(DataContext context) : base(context)
        {
        }

        public Task<Question> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Question> QueryMarkedQuestions(int userId, string search, IList<string> tags, bool answeredOnly, int page, int size)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Question> QueryQuestions(int? userId, string search, IList<string> tags, bool answeredOnly, int page, int size)
        {
            throw new System.NotImplementedException();
        }
    }
}
