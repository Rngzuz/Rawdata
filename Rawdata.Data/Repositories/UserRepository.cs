﻿using System.Collections.Generic;
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

        public virtual void Add(User user)
        {
            Context.Set<User>().Add(user);
        }

        public virtual async Task<IEnumerable<User>> GetAllAsync()
        {
            
            return await Context.Users.ToListAsync();
        }

        public virtual void Update(User user)
        {
            Context.Set<User>().Update(user);
        }

        public virtual void Remove(User user)
        {
            Context.Set<User>().Remove(user);
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

        public async Task<User> GetUserByEmail(string email)
        {
            var db = Context.Database.GetDbConnection();

            using (var cmd  = db.CreateCommand())
            {
                cmd.CommandText = "select * from get_user_by_email(@email)";
                cmd.Parameters.Add(new NpgsqlParameter("email", email));
                
                await cmd.ExecuteNonQueryAsync();
            }

            throw new System.NotImplementedException();
        }

        public DeactivatedUser GetDeactivatedUserByEmail(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}