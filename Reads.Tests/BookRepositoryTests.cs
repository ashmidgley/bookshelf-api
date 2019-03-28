using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Reads.Models;
using Xunit;

namespace Reads.Tests
{
    public class BookRepositoryTests
    {
        private static IEnumerable<Book> InitialBooks => new[]
        {
            new Book { Id = 1, Image = "testing", CategoryId = 1, StartedOn = new DateTime(2019,1,1), FinishedOn = new DateTime(2019,1,11), PageCount = 112, Title = "Testing", Author = "Testing", Summary = "Testing", Removed = false },
            new Book { Id = 2, Image = "testing", CategoryId = 1, StartedOn = new DateTime(2019,1,1), FinishedOn = new DateTime(2019,1,11), PageCount = 112, Title = "Testing", Author = "Testing", Summary = "Testing", Removed = false }
        };

        private IBookRepository TestRepository
        {
            get
            {
                var dbContextMock = new DbContextMock<ReadsContext>();
                dbContextMock.CreateDbSetMock(x => x.Books, InitialBooks);
                return new BookRepository(dbContextMock.Object);
            }
        }

        [Fact(DisplayName = "Should get multiple books.")]
        public void ShouldGetMultipleBooks()
        {
            var books = TestRepository.GetAll().Result;
            books.Should().HaveCount(InitialBooks.Count());
        }

        [Fact(DisplayName = "Should not get book.")]
        public void ShouldNotGetBook()
        {
            var book = TestRepository.Get(5).Result;
            Assert.Null(book);
        }

        [Fact(DisplayName = "Should get book.")]
        public void ShouldGetBook()
        {
            var book = TestRepository.Get(1).Result;
            Assert.NotNull(book);
        }
    }
}
