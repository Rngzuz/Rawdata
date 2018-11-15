using System.Threading.Tasks;
using Rawdata.Data.Models;

namespace Rawdata.Data.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> GetAuthorById(int id);
    }
}
