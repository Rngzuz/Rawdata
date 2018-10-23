using System.Collections;
using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        void FavoritePost(int userId, int postId, string note);

        //TODO Posts needs an extra property called tags
        IEnumerable<Post> FilterByTags(IList<string> tags);

        IEnumerable<Post> FilterByWords(int userId, string searchString, IList<string> tags);

        IEnumerable<FavoritePost> GetFavoritePosts(int userId);

        IEnumerable<Post> GetLinkedPosts(int postId);
    }
}