using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public User RegisterUser(string name, string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public void ReactivateUser(int deactivatedUserId)
        {
            throw new System.NotImplementedException();
        }

        public void DeactivateUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public DeactivatedUser GetDeactivatedUserByEmail(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}