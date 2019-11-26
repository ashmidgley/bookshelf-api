using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class RatingRepositoryTests
    {
        DbContextOptions<BookshelfContext> options;

        public RatingRepositoryTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(databaseName: "Bookshelf")
                .Options;

            using (var context = new BookshelfContext(options))
            {
                context.Ratings.Add(new Rating { Id = 1, Description = "Mild", Code = "🔥" });
                context.Ratings.Add(new Rating { Id = 2, Description = "Hot", Code = "🔥🔥🔥" });
                context.SaveChanges();
            }

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(context.Ratings.CountAsync().Result, 2);
            }
        }

        [Test]
        public void GetAllTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                Assert.AreEqual(repository.GetAll().Result.Count, 2);
            }
        }

        [Test]
        public void GetSingleTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                Assert.NotNull(repository.Get(1).Result);
                Assert.Null(repository.Get(5).Result);
            }
        }
    }
}