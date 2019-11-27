using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookshelfContext _context;

        public CategoryRepository(BookshelfContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<Category> Get(int id)
        {
            return await _context.Categories
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public int Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category.Id;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChangesAsync();
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChangesAsync();
        }
    }
}
