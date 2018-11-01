using System.Collections.Generic;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll(int? userId, string search, int page, int size);

        Task<IEnumerable<Comment>> GetAllMarked(int userId, string search, int page, int size);
    }
}
