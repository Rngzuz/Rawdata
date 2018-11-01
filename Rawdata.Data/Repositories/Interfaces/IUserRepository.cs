using System.Collections.Generic;
using System.Threading.Tasks;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> RegisterUser(string name, string email, string password);

        Task<User> GetUserByEmail(string email);

        Task<IEnumerable<MarkedComment>> GetMarkedComments(int userId);
        
        Task<IEnumerable<MarkedPost>> GetMarkedPosts(int userId);

        Task<IEnumerable<Search>> GetSearches(int userId);
    }
}
