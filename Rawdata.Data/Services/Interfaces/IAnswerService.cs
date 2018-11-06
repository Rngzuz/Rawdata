using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IAnswerService
    {

        Task<Answer> GetById(int id);

        IQueryable<Answer> QueryAnswers(int? userId, string search, int page, int size);

        IQueryable<Answer> QueryMarkedAnswers(int? userId, string search, int page, int size);

    }
}
