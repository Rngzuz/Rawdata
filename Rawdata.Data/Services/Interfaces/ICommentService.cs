using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentById(int id);

        IQueryable<Comment> QueryComments(int? userId, string search, int page, int size);

        IQueryable<Comment> QueryMarkedComments(int? userId, string search, int page, int size);
    }
}
