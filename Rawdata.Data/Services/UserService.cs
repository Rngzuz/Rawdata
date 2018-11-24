using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DataContext context) : base(context)
        {
        }

        public virtual Task<User> GetUserById(int id)
        {
            return Context.Users
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await Context.Users
                .SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> RegisterUser(User user)
        {
            string password = PasswordHelper.CreatePassword(user.Password);

            var newUser = await Context.Users
                .FromSql($"select * from add_user({user.DisplayName}, {user.Email}, {password})")
                .SingleOrDefaultAsync();

            return newUser;
        }

        public async Task<IList<Search>> GetUserHistory(int userId)
        {
            return await Context.Searches.Where(search => search.UserId == userId).ToListAsync();
        }

        public IQueryable<MarkedPost> GetMarkedPosts(int? userId)
        {
            return Context.MarkedPosts.Where(mp => mp.UserId == userId);
        }

        public IQueryable<MarkedComment> GetMarkedComments(int userId)
        {
            return Context.MarkedComments.Where(mc => mc.UserId == userId);
        }


        public void DeleteUser(User user)
        {
            Context.Users
                .FromSql($"delete from users where id={user.Id}")
                .SingleOrDefaultAsync();
        }


        public IQueryable<MarkedPost> ToggleMarkedPost(int? userId, int postId, string note)
        {
            return Context.MarkedPosts
                .FromSql($"select * from toggle_marked_post({userId}, {postId}, {note})");
        }

        public async Task<bool> UpdateMarkedPostNote(int? userId, int postId, string note)
        {
            var markedPost =
                await Context.MarkedPosts.SingleOrDefaultAsync(mp => mp.UserId == userId && mp.PostId == postId);

            if (markedPost == null)
            {
                return false;
            }

            markedPost.Note = note;
            await Context.SaveChangesAsync();

            return true;
        }
    }
}
