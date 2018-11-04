using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> RegisterUser(User user);
    }
}
