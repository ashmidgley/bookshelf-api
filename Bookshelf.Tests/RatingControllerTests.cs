using System.Collections.Generic;
using System.Linq;
using System.Net;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Bookshelf.Controllers;
using Bookshelf.Models;
using Bookshelf.Validators;
using Xunit;

namespace Bookshelf.Tests
{
    public class RatingControllerTests
    {
        private static IEnumerable<Rating> InitialRatings => new[]
        {
            new Rating { Id = 1, Description = "Mild", Code = "🔥" },
            new Rating { Id = 2, Description = "Hot", Code = "🔥🔥🔥" },
        };

        private RatingValidator TestValidator => new RatingValidator();

        private IRatingRepository TestRepository
        {
            get
            {
                var dbContextMock = new DbContextMock<BookshelfContext>();
                dbContextMock.CreateDbSetMock(x => x.Ratings, InitialRatings);
                return new RatingRepository(dbContextMock.Object);
            }
        }

        [Fact(DisplayName = "Should get multiple ratings.")]
        public void ShouldGetMultipleRatings()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            var response = controller.Get();
            if (response.Value.Except(InitialRatings).Any())
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should post Rating.")]
        public void ShouldPostRating()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            var Rating = new Rating
            {
                Description = "Testing",
                Code = "testing"
            };
            var response = controller.Post(Rating);
            Rating.Id = 3;
            if (response.Value.Equals(Rating))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not post rating.")]
        public void ShouldNotPostRating()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            var Rating = new Rating();
            var response = controller.Post(Rating);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Should update rating.")]
        public void ShouldUpdateRating()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            var Rating = new Rating
            {
                Id = 1,
                Description = "testing",
                Code = "Testing"
            };
            var response = controller.Put(Rating);
            if (response.Value.Equals(Rating))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not update rating.")]
        public void ShouldNotUpdateRating()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            var Rating = new Rating();
            var response = controller.Put(Rating);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Should delete rating.")]
        public void ShouldDeleteRating()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            int id = 1;
            var response = controller.Delete(id);
            if (response.Value.Id == id) {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not delete rating.")]
        public void ShouldNotDeleteRating()
        {
            var controller = new RatingsController(TestRepository, TestValidator);
            int id = 5;
            var response = controller.Delete(id);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
