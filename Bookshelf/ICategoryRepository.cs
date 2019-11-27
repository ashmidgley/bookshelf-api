using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookshelf
{
    public interface ICategoryRepository
    {
        int Add(Category category);
        void Delete(Category category);
        Task<Category> Get(int id);
        Task<List<Category>> GetAll();
        void Update(Category category);
    }
}
