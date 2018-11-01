using System.Threading.Tasks;

namespace Rawdata.Data.Services
{
    public class ServiceBase
    {
        protected readonly DataContext Context;

        public ServiceBase(DataContext context)
        {
            Context = context;
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
