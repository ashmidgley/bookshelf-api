using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookshelf
{
    public interface IRatingRepository
    {
        Task<int> Add(Rating Rating);
        Task Delete(Rating Rating);
        Task<Rating> Get(int id);
        Task<List<Rating>> GetAll();
        Task Update(Rating Rating);
    }
}
