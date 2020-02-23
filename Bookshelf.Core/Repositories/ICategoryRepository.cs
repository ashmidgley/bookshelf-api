using System.Collections.Generic;

namespace Bookshelf.Core
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetUserCategories(int userId);
        Category GetCategory(int id);
        int Add(Category category);
        void Update(Category category);
        void Delete(int id);
        bool CategoryExists(int id);
    }
}
