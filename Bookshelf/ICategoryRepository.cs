using System.Collections.Generic;
using System.Threading.Tasks;
using Bookshelf.Models;

namespace Bookshelf
{
    public interface ICategoryRepository
    {
        Category Add(Category category);
        void Delete(Category category);
        Task<Category> Get(int id);
        Task<List<Category>> GetAll();
        void Update(Category category);
    }
}
