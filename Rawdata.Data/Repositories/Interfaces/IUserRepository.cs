using System.Collections.Generic;
using System.Threading.Tasks;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> RegisterUser(string name, string email, string password);

        void Remove(User user);

        Task<User> GetUserByEmail(string email);

        Task<User> GetById(int id);

        Task<IEnumerable<FavoriteComment>> GetFavoriteComments(int userId);

        Task<IEnumerable<FavoritePost>> GetFavoritePosts(int userId);
        
    }
}