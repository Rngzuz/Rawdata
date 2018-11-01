using System.Collections.Generic;
using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(string name, string email, string password);

        Task<User> GetUserByEmail(string email);

        Task<IEnumerable<Search>> GetSearches(int userId);

        Task<User> GetById(int id);
    }
}
