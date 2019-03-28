using Microsoft.EntityFrameworkCore;
using Reads.Models;

namespace Reads
{
    public class ReadsContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public ReadsContext(DbContextOptions<ReadsContext> options)
            : base(options)
        { }

        public ReadsContext()
        { }
    }
}
