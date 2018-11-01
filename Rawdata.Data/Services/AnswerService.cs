using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class AnswerService : ServiceBase, IAnswerService
    {
        public AnswerService(DataContext context) : base(context)
        {
        }

        public Task<Answer> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Answer> QueryAnswers(int? userId, string search, IList<string> tags, bool answeredOnly, int page, int size)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Answer> QueryMarkedAnswers(int userId, string search, IList<string> tags, bool answeredOnly, int page, int size)
        {
            throw new System.NotImplementedException();
        }
    }
}
