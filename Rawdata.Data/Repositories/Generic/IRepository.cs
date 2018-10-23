using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rawdata.Data.Repositories.Generic
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        void Update(T entity);

        void Remove(T entity);

        Task SaveChangesAsync();
    }
}