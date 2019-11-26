using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Tests
{
    public class RatingRepositoryTests
    {
        private readonly DbContextOptions<BookshelfContext> options;
        private RatingValidator Validator => new RatingValidator();

        public RatingRepositoryTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new BookshelfContext(options))
            {
                context.Ratings.Add(new Rating { Description = "Mild", Code = "🔥" });
                context.Ratings.Add(new Rating { Description = "Hot", Code = "🔥🔥🔥" });
                context.SaveChanges();
            }

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(2, context.Ratings.CountAsync().Result);
            }
        }

        /* Repository tests. */

        [Test]
        public void GetAllRepositoryTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                Assert.AreEqual(2, repository.GetAll().Result.Count);
            }
        }

        [Test]
        public void GetSingleRepositoryTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
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
                var repository = new RatingRepository(context);
                var controller = new RatingsController(repository, Validator);
                var ratings = controller.Get().Value.ToList();
                Assert.AreEqual(2, ratings.Count);
            }
        }

        [Test]
        public void PostControllerTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                var controller = new RatingsController(repository, Validator);
                var rating = new Rating
                {
                    Description = "Sci-fi",
                    Code = "🚀"
                };
                var response = controller.Post(rating);
                rating.Id = 3;
                Assert.AreEqual(rating, response.Value);
            }
        }
    }
}