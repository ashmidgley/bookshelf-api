using System;
using System.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Reads.Tests
{
    public class BookRepositoryTests
    {
        [Fact(DisplayName = "Should get all the books")]
        public void Should_Get_All_Books()
        {
            var repo = BookRepository;
            repo.GetAll().Should().NotBeNull();
        }

        private IBookRepository BookRepository
        {
            get
            {
                var builder = new DbContextOptionsBuilder<ReadsContext>();
                builder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                DbContextOptions<ReadsContext> options = builder.Options;
                ReadsContext readContext = new ReadsContext(options);
                readContext.Database.EnsureDeleted();
                readContext.Database.EnsureCreated();
                return new BookRepository(readContext);
            }
        }
    }
}
