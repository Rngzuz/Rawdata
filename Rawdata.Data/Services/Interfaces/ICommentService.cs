using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentById(int id);

        IQueryable<MarkedComment> ToggleMarkedComment(int? userId, int commentId, string note);

        Task<bool> UpdateMarkedCommentNote(int? userId, int commentId, string note);
    }
}
