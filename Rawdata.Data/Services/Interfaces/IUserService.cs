using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> RegisterUser(User user);
        IQueryable<MarkedPost> ToggleMarkedPost(int? userId, int postId, string note);
        Task<bool> UpdateMarkedPostNote(int? userId, int postId, string note);
        IQueryable<MarkedComment> GetMarkedComments(int userId);
        IQueryable<MarkedPost> GetMarkedPosts(int userId);
        Task<IList<Search>> GetUserHistory(int userId);
    }
}
