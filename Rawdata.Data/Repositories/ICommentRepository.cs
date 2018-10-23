using System.Collections;
using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<FavoriteComment> GetFavoriteCommentsByUserId(int userId);

        IEnumerable<DeactivatedFavoriteComment> GetFavoriteDeactivatedCommentsByUserId(int userId);
    }
}