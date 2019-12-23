using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api
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
                .SingleAsync(c => c.Id == id);
        }

        public async Task<int> Add(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task Update(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
