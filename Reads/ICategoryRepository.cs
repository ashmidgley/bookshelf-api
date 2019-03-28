using System.Collections.Generic;
using System.Threading.Tasks;
using Reads.Models;

namespace Reads
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        void Delete(Category category);
        Task<Category> Get(int id);
        Task<List<Category>> GetAll();
        void Update(Category category);
    }
}
