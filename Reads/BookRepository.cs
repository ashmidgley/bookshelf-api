using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reads.Models;

namespace Reads
{
    public class BookRepository : IBookRepository
    {
        private readonly ReadsContext _context;

        public BookRepository(ReadsContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _context.Books
                .ToListAsync();
        }

        public async Task<Book> Get(int id)
        {
            return await _context.Books
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChangesAsync();
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChangesAsync();
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChangesAsync();
        }
    }
}
