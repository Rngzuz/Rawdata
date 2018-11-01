using System.Collections.Generic;
using Rawdata.Data.Models;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IFavoritePostRepository
    {
        void FavoritePost(int userId, int postId, string note);

        void UnfavoritePost(int userId, int postId);

        IEnumerable<MarkedPost> GetFavoritePosts(int userId);
    }
}