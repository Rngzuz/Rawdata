using System.Linq;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface ICommentService
    {
        IQueryable<Comment> QueryComments(int? userId, string search, int page, int size);

        IQueryable<Comment> QueryMarkedComments(int userId, string search, int page, int size);
    }
}
