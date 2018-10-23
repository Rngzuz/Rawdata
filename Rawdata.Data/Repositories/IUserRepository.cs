using System.Collections;
using System.Collections.Generic;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;

namespace Rawdata.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User RegisterUser(string name, string email, string password);

        void ReactivateUser(int deactivatedUserId);

        void DeactivateUser(int userId);
        
        User GetUserByEmail(string email);

        DeactivatedUser GetDeactivatedUserByEmail(string email);
    }
}