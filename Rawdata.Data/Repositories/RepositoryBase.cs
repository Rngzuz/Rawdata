namespace Rawdata.Data.Repositories.Generic
{
    public class RepositoryBase
    {
        protected readonly DataContext Context;

        public RepositoryBase(DataContext context)
        {
            Context = context;
        }
    }
}
