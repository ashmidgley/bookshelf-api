using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reads.Models;

namespace Reads
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ReadsContext _context;

        public CategoryRepository(ReadsContext context)
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

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChangesAsync();
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
