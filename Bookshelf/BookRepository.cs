using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                .SingleAsync(b => b.Id == id);
        }

        public async Task<int> Add(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task Update(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
