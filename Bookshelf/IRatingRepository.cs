using System.Collections.Generic;
using System.Threading.Tasks;
using Bookshelf.Models;

namespace Bookshelf
{
    public interface IRatingRepository
    {
        int Add(Rating Rating);
        void Delete(Rating Rating);
        Task<Rating> Get(int id);
        Task<List<Rating>> GetAll();
        void Update(Rating Rating);
    }
}
