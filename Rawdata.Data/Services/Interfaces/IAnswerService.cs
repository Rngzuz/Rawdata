using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<Answer> GetAnswerById(int id);
    }
}
