using System.Collections.Generic;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(string name, string email, string password);

        Task<User> GetUserByEmail(string email);

        Task<IEnumerable<MarkedComment>> GetMarkedComments(int userId);

        Task<IEnumerable<MarkedPost>> GetMarkedPosts(int userId);

        Task<IEnumerable<Search>> GetSearches(int userId);

        Task<User> GetById(int id);
    }
}
