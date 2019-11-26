using NUnit.Framework;
using Bookshelf;
using System;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class BookRepositoryTests
    {
        DbContextOptions<BookshelfContext> options;

        public BookRepositoryTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(databaseName: "Bookshelf")
                .Options;

            using (var context = new BookshelfContext(options))
            {
                context.Books.Add(
                    new Book 
                    { 
                        Id = 1,
                        Image = "fight-club.png",
                        CategoryId = 1,
                        StartedOn = new DateTime(2019,1,1),
                        FinishedOn = new DateTime(2019,4,12),
                        PageCount = 224,
                        Title = "Fight Club",
                        Author = "Chuck Palahniuk",
                        Summary = "Chuck Palahniuk showed himself to be his generation’s most visionary satirist in this, his first book..."
                    }
                );
                context.Books.Add(
                    new Book
                    {
                        Id = 2,
                        Image = "choke.png",
                        CategoryId = 1,
                        StartedOn = new DateTime(2019,5,5),
                        FinishedOn = new DateTime(2019,9,27),
                        PageCount = 293,
                        Title = "Choke",
                        Author = "Chuck Palahniuk",
                        Summary = "Victor Mancini, a medical-school dropout, is an antihero for our deranged times..."
                    }
                );
                context.SaveChanges();
            }

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(context.Books.CountAsync().Result, 2);
            }
        }

        [Test]
        public void GetAllTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                Assert.AreEqual(repository.GetAll().Result.Count, 2);
            }
        }

        [Test]
        public void GetSingleTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                Assert.NotNull(repository.Get(1).Result);
                Assert.Null(repository.Get(5).Result);
            }
        }
    }
}