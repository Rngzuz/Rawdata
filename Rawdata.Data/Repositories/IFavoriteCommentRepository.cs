using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories
{
    public interface IFavoriteCommentRepository : IRepository<FavoriteComment>
    {
        void FavoriteComment(int userId, int commentId, string note);


    }
}