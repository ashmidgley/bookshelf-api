using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Category> GetAll()
        {
            return _context.Categories
                .ToList();
        }

        public Category Get(int id)
        {
            return _context.Categories
                .SingleOrDefault(c => c.Id == id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            var old = _context.Categories
                .SingleOrDefault(c => c.Id == category.Id);

            if(old == null) return;

            old.Code = category.Code;
            old.Description = category.Description;
            _context.SaveChanges();
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
