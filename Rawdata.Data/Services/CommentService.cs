using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class CommentService : ServiceBase, ICommentService
    {
        public CommentService(DataContext context) : base(context)
        {
        }

        public IQueryable<Comment> QueryComments(int? userId, string search, int page, int size)
        {
            return Context.Comments
                .FromSql($"select * from query_comments({search}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size
        }

        public IQueryable<Comment> QueryMarkedComments(int userId, string search, int page, int size)
        {
            return Context.Comments
                .FromSql($"select * from query_marked_comments({search}, {userId})")
                .Skip(size * (page - 1))
                .Take(size);
        }
    }
}
