using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface ICategoryRepository
    {
        Task<int> Add(Category category);
        Task Delete(Category category);
        Task<Category> Get(int id);
        Task<List<Category>> GetAll();
        Task Update(Category category);
    }
}
