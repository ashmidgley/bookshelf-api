using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bookshelf.Models;

namespace Bookshelf
{
    public class BookRepository : IBookRepository
    {
        private readonly BookshelfContext _context;

        public BookRepository(BookshelfContext context)
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

        public int Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book.Id;
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
