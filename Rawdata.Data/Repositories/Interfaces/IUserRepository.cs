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

<<<<<<< HEAD
        Task<User> GetById(int id);

        Task<IEnumerable<FavoriteComment>> GetFavoriteComments(int userId);
=======
        Task<IEnumerable<FavoriteComment>> GetFavoriteComments(int id);
>>>>>>> a0ab269601d6658c990ad4ecb7cb189dfcc452bc

        Task<IEnumerable<FavoritePost>> GetFavoritePosts(int userId);
        
    }
}