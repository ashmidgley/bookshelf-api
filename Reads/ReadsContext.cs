using Microsoft.EntityFrameworkCore;
using Reads.Models;

namespace Reads
{
    public class ReadsContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ReadsContext(DbContextOptions<ReadsContext> options)
            : base(options)
        { }
    }
}
