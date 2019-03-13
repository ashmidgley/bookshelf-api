using System.Collections.Generic;
using Reads.Models;

namespace Reads
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Get(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
