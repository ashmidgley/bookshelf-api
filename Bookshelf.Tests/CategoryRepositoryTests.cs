using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Bookshelf.Models;
using Xunit;

namespace Bookshelf.Tests
{
    public class CategoryRepositoryTests
    {
        private static IEnumerable<Category> InitialCategories => new[]
        {
            new Category { Id = 1, Description = "Testing", Code = "testing" },
            new Category { Id = 2, Description = "Testing", Code = "testing" },
        };

        private ICategoryRepository TestRepository
        {
            get
            {
                var dbContextMock = new DbContextMock<BookshelfContext>();
                dbContextMock.CreateDbSetMock(x => x.Categories, InitialCategories);
                return new CategoryRepository(dbContextMock.Object);
            }
        }

        [Fact(DisplayName = "Should get multiple categories.")]
        public void ShouldGetMultipleBooks()
        {
            var categories = TestRepository.GetAll().Result;
            categories.Should().HaveCount(InitialCategories.Count());
        }

        [Fact(DisplayName = "Should not get category.")]
        public void ShouldNotGetBook()
        {
            var category = TestRepository.Get(5).Result;
            Assert.Null(category);
        }

        [Fact(DisplayName = "Should get category.")]
        public void ShouldGetBook()
        {
            var category = TestRepository.Get(1).Result;
            Assert.NotNull(category);
        }
    }
}
