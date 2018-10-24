using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Post> FilterByTags(IList<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> FilterByWords(int userId, string searchString, IList<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> GetLinkedPosts(int postId)
        {
            throw new System.NotImplementedException();
        }
    }
}