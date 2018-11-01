using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class CommentRepository : RepositoryBase, ICommentRepository
    {
        public CommentRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetAll(int? userId, string search, int page, int size)
        {
            var query = Context.Comments
                .FromSql($"select * from get_all_comments({search}, {userId})")
                .Skip(size * (page - 1)) // Skip records based on page number
                .Take(size); // Limit the result set to the size

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllMarked(int userId, string search, int page, int size)
        {
            var query = Context.Comments
                .FromSql($"select * from get_all_marked_comments({search}, {userId})")
                .Skip(size * (page - 1))
                .Take(size);

            return await query.ToListAsync();
        }
    }
}
