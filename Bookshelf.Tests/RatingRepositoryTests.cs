using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Bookshelf.Models;
using Xunit;

namespace Bookshelf.Tests
{
    public class RatingRepositoryTests
    {
        private static IEnumerable<Rating> InitialRatings => new[]
        {
            new Rating { Id = 1, Description = "Mild", Code = "🔥" },
            new Rating { Id = 2, Description = "Hot", Code = "🔥🔥🔥" },
        };

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
        public void ShouldGetMultipleBooks()
        {
            var Ratings = TestRepository.GetAll().Result;
            Ratings.Should().HaveCount(InitialRatings.Count());
        }

        [Fact(DisplayName = "Should not get rating.")]
        public void ShouldNotGetBook()
        {
            var Rating = TestRepository.Get(5).Result;
            Assert.Null(Rating);
        }

        [Fact(DisplayName = "Should get rating.")]
        public void ShouldGetBook()
        {
            var Rating = TestRepository.Get(1).Result;
            Assert.NotNull(Rating);
        }
    }
}
