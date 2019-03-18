using System.Collections.Generic;
using System.Threading.Tasks;
using Reads.Models;

namespace Reads
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        Task<Category> Get(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
