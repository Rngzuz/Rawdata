using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Data.Services
{
    public class AuthorService : BaseService, IAuthorService
    {
        public AuthorService(DataContext context) : base(context)
        {
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await Context.Authors.SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}
