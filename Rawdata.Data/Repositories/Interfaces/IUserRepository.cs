using System.Collections.Generic;
using System.Threading.Tasks;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User RegisterUser(string name, string email, string password);

        void Remove(int userId);
        
        User GetUserByEmail(string email);

        Task<IEnumerable<FavoriteComment>> GetFavoriteComments(int userId);

        Task<ICollection<FavoritePost>> GetFavoritePosts(int userId);
        
    }
}