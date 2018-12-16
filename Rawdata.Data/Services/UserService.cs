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

        public virtual Task<User> GetUserById(int? id)
        {
            return Context.Users
                .Where(u => u.Id == id)
                .Include(u => u.MarkedComments)
                    .ThenInclude(mc => mc.Comment)
                        .ThenInclude(c => c.Author)
                .Include(u => u.MarkedPosts)
                    .ThenInclude(mp => mp.Post)
                        .ThenInclude(p => p.Author)
                .Include(u => u.Searches)
                .SingleOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await Context.Users
                .Where(u => u.Email.ToLower() == email.ToLower())
                .SingleOrDefaultAsync();
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
            return Context.MarkedPosts
                .Where(mp => mp.UserId == userId)
                .Include(mp => mp.Post)
                .ThenInclude(p => p.Author)
                .OrderByDescending(mp => mp.Post.Score);
        }

        public IQueryable<MarkedComment> GetMarkedComments(int userId)
        {
            return Context.MarkedComments
                .Where(mc => mc.UserId == userId)
                .OrderByDescending(mp => mp.Comment.Score);;
        }

        public async Task SaveToSearchHistory(int? userId, string searchText)
        {
            if (userId.HasValue)
            {
                // using sql so we can ignore dupolicate conflicts
                try
                {
                    await Context.Database
                        .ExecuteSqlCommandAsync(
                            $"insert into searches values ({userId}, {searchText}) on conflict do nothing");
                }
                catch
                {
                }
            }
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