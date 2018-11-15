using System.Threading.Tasks;

namespace Rawdata.Data.Services
{
    public class BaseService
    {
        public readonly DataContext Context;

        public BaseService(DataContext context)
        {
            Context = context;
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
