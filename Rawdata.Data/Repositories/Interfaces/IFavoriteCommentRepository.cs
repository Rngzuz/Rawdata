using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IFavoriteCommentRepository : IRepository<MarkedComment>
    {
        void FavoriteComment(int userId, int commentId, string note);

        void UnfavoriteComment(int userId, int commentId);

        IEnumerable<MarkedComment> GetFavoriteCommentsByUserId(int userId);
    }
}