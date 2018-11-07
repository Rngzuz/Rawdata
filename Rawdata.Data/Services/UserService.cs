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

<<<<<<< HEAD
        public void DeleteUser(User user)
        {
            Context.Users
                .FromSql($"delete from users where id={user.Id}")
                .SingleOrDefaultAsync();


=======
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
>>>>>>> b509b5a123814a220766fcf4cdac4adca0731f4d
        }
    }
}
