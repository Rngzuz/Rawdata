using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(DataContext context) : base(context)
        {
        }

        public virtual Task<User> GetById(int id)
        {
            return Context.Users.SingleOrDefaultAsync(a => a.Id == id);
        }

        public virtual async Task<IEnumerable<MarkedComment>> GetMarkedComments(int id)
        {
            return await Context.MarkedComments
                .Include(mark => mark.User)
                .Include(mark => mark.Comment)
                .Where(mark => mark.UserId == id)
                .ToListAsync();
        }


        public virtual IEnumerable<MarkedComment> GetMarkedCommentsByCommentId(int commentid)
        {
            return Context.MarkedComments.Where(mark => (mark.CommentId == commentid && mark.UserId == 1)).ToList();
        }

        public virtual async Task<IEnumerable<MarkedPost>> GetMarkedPosts(int id)
        {
            return await Context.MarkedPosts
                    .Where(a => a.UserId == id)
                    .ToListAsync();
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
            Context.Database.ExecuteSqlCommand("select * register_user @p0, @p1, @p2",
                parameters: new object[] {name, email, password});

            return await GetUserByEmail(email);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await Context.Users.SingleOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
        }

        public virtual async Task<IEnumerable<Search>> GetSearches(int userId)
        {
            var db = Context.Database.GetDbConnection();

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
