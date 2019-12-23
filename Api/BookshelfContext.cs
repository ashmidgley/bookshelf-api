using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class BookshelfContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<ApiKey> ApiKeys { get; set; }

        public BookshelfContext(DbContextOptions<BookshelfContext> options)
            : base(options)
        { }

        public BookshelfContext()
        { }
    }
}
