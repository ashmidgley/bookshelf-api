using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Tests
{
    public class BookRepositoryTests
    {
        private readonly DbContextOptions<BookshelfContext> options;
        private BookValidator Validator => new BookValidator();

        public BookRepositoryTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            using (var context = new BookshelfContext(options))
            {
                context.Books.Add(
                    new Book 
                    { 
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
            };

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(2, context.Books.CountAsync().Result);
            }
        }

        /* Repository tests. */

        [Test]
        public void GetAllRepositoryTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                Assert.AreEqual(2, repository.GetAll().Result.Count);
            }
        }

        [Test]
        public void GetSingleRepositoryTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                Assert.NotNull(repository.Get(1).Result);
                Assert.Null(repository.Get(5).Result);
            }
        }

        /* Controller tests. */

        [Test]
        public void GetAllControllerTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                var controller = new BooksController(repository, Validator);
                var books = controller.Get().Value.ToList();
                Assert.AreEqual(2, books.Count);
            }
        }
    }
}