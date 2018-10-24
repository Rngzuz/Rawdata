using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> FilterByTags(IList<string> tags);

        IEnumerable<Post> FilterByWords(int userId, string searchString, IList<string> tags);
       
        IEnumerable<Post> GetLinkedPosts(int postId);
    }
}