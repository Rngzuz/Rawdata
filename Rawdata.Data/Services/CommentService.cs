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

        public IQueryable<Comment> QueryComments(int? userId, string search, int page, int size)
        {
            return Context.Comments
                .FromSql($"select * from query_comments({search}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }

        public IQueryable<Comment> QueryMarkedComments(int? userId, string search, int page, int size)
        {
            return Context.Comments
                .FromSql($"select * from query_marked_comments({search}, {userId})")
                .Skip(size * (page - 1))
                .Take(size);
        }

        public IQueryable<MarkedComment> ToggleMarkedComment(int? userId, int commentId, string note)
        {
            return Context.MarkedComments
                .FromSql($"select * from toggle_marked_comment({userId}, {commentId}, {note})");
        }
    }
}
