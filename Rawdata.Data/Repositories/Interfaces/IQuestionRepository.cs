using System.Collections.Generic;
using Rawdata.Data.Models;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> FilterByTags(IList<string> tags);

        IEnumerable<Question> FilterByWords(int userId, string searchString, IList<string> tags);

        IEnumerable<Question> GetLinkedPosts(int postId);
    }
}
