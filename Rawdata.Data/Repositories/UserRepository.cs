using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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

        public virtual Task<User> GetById(int id)
        {
            return Context.Users.SingleOrDefaultAsync(a => a.Id == id);
        }

        public virtual async Task<IEnumerable<FavoriteComment>> GetFavoriteComments(int id)
        {
            return await Context.FavoriteComments.Where(a => a.UserId == id).ToListAsync();
        }

        public virtual async Task<IEnumerable<FavoritePost>> GetFavoritePosts(int id)
        {
            return await Context.FavoritePosts.Where(a => a.UserId == id).ToListAsync();
        }

        public virtual void Add(User user)
        {
            Context.Users.Add(user);
        }

        public virtual async Task<IEnumerable<User>> GetAllAsync()
        {

            return await Context.Users.ToListAsync();
        }

        public virtual void Update(User user)
        {
            Context.Users.Update(user);
        }

        public virtual void Remove(User user)
        {
            Context.Users.Remove(user);
        }

        public async Task<User> RegisterUser(string name, string email, string password)
        {
            var newUser = new User { DisplayName = name, Email = email, Password = password };

            Context.Users.Add(newUser);

            await Context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var db = Context.Database.GetDbConnection();

            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = "select * from get_user_by_email(@email)";
                cmd.Parameters.Add(new NpgsqlParameter("email", email));

                await cmd.ExecuteNonQueryAsync();
            }

            throw new System.NotImplementedException();
        }

        public virtual async Task<IEnumerable<Search>> GetSearches(User user)
        {
            var db = Context.Database.GetDbConnection();

            int userId = user.Id;
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = "select * from get_users_search_history(@userId)";
                cmd.Parameters.Add(new NpgsqlParameter("userId", userId));

                await cmd.ExecuteNonQueryAsync();
            }

            throw new System.NotImplementedException();
        }
    }
}
