using Microsoft.EntityFrameworkCore;
using Bookshelf.Models;

namespace Bookshelf
{
    public class BookshelfContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

        public BookshelfContext(DbContextOptions<BookshelfContext> options)
            : base(options)
        { }

        public BookshelfContext()
        { }
    }
}
