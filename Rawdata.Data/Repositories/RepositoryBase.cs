using System.Threading.Tasks;

namespace Rawdata.Data.Repositories
{
    public class RepositoryBase
    {
        protected readonly DataContext Context;

        public RepositoryBase(DataContext context)
        {
            Context = context;
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
