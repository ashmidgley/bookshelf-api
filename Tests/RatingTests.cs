using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class RatingTests
    {
        private readonly DbContextOptions<BookshelfContext> options;
        private RatingValidator Validator => new RatingValidator();

        public RatingTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new BookshelfContext(options))
            {
                context.Ratings.Add(new Rating { Description = "Mild", Code = "🔥" });
                context.Ratings.Add(new Rating { Description = "Medium", Code = "🔥🔥" });
                context.SaveChanges();
            }

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(2, context.Ratings.CountAsync().Result);
            }
        }

        [Test]
        public void GetAllTest()
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
        public void PostTest()
        {
            var ratingSuccess = new Rating { Description = "Hot", Code = "🔥🔥🔥" };
            var ratingFail = new Rating();
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                var controller = new RatingsController(repository, Validator);
                var responseOne = controller.Post(ratingSuccess);
                ratingSuccess.Id = 3;
                Assert.AreEqual(ratingSuccess, responseOne.Value);
                var responseTwo = controller.Post(ratingFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
                Assert.IsNull(responseTwo.Value);
            }
        }

        [Test]
        public void UpdateTest()
        {
            var ratingSuccess = new Rating { Id = 1, Description = "Extra-mild", Code = "🔥" };
            var ratingFail = new Rating();
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                var controller = new RatingsController(repository, Validator);
                var responseOne = controller.Put(ratingSuccess);
                Assert.AreEqual(ratingSuccess, responseOne.Value);
                var responseTwo = controller.Put(ratingFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
                Assert.IsNull(responseTwo.Value);
            }
        }

        [Test]
        public void DeleteTest()
        {
            int idSuccess = 2;
            int idFail = 5;
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                var controller = new RatingsController(repository, Validator);
                var responseOne = controller.Delete(idSuccess);
                Assert.AreEqual(idSuccess, responseOne.Value.Id);
                var responseTwo = controller.Delete(idFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
                Assert.IsNull(responseTwo.Value);
            }
            var rating = new Rating { Description = "Medium", Code = "🔥🔥" };
            using (var context = new BookshelfContext(options))
            {
                var repository = new RatingRepository(context);
                var controller = new RatingsController(repository, Validator);
                controller.Post(rating);
            }
        }
    }
}