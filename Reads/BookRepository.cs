using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Book> GetAll()
        {
            return _context.Books
                .ToList();
        }

        public Book Get(int id)
        {
            return _context.Books
                .SingleOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            var old = _context.Books
                .SingleOrDefault(b => b.Id == book.Id);

            if (old == null) return;

            old.Author = book.Author;
            old.CategoryId = book.CategoryId;
            old.FinishedOn = book.FinishedOn;
            old.Image = book.Image;
            old.PageCount = book.PageCount;
            old.Removed = book.Removed;
            old.StartedOn = book.StartedOn;
            old.Title = book.Title;
            old.Summary = book.Summary;
            _context.SaveChanges();
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
    }
}
