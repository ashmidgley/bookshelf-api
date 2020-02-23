using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.Core
{
    public class BookRepository : IBookRepository
    {
        private readonly BookshelfContext _context;
        private readonly IBookHelper _helper;

        public BookRepository(BookshelfContext context, IBookHelper helper)
        {
            _context = context;
            _helper = helper;
        }

        public IEnumerable<BookDto> GetUserBooks(int userId)
        {
            return _context.Books
                .Where(b => b.UserId == userId)
                .Select(b => _helper.ToBookDto(b));
        }

        public BookDto GetBook(int id)
        {
            var book = _context.Books
                .Single(b => b.Id == id);
            return _helper.ToBookDto(book);
        }

        public int Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book.Id;
        }

        public void Update(BookDto dto)
        {
            var book = _helper.ToBook(dto);
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = _context.Books
                .Single(b => b.Id == id);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public bool BookExists(int id)
        {
            return _context.Books
                .Any(x => x.Id == id);
        }
    }
}
