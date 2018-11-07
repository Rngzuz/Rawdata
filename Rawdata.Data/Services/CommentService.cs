using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(DataContext context) : base(context)
        {
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await Context.Comments
                .Include(c => c.Author)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<MarkedComment> ToggleMarkedComment(int? userId, int commentId, string note)
        {
            return Context.MarkedComments
                .FromSql($"select * from toggle_marked_comment({userId}, {commentId}, {note})");
        }

        public async Task<bool> UpdateMarkedCommentNote(int? userId, int commentId, string note)
        {
            var markedComment =
               await Context.MarkedComments.SingleOrDefaultAsync(mp => mp.UserId == userId && mp.CommentId == commentId);

            if (markedComment == null)
            {
                return false;
            }

            markedComment.Note = note;
            await Context.SaveChangesAsync();
        
            return true;
        }
    }
}
