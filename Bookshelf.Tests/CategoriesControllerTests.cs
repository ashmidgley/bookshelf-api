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
    public class CategoryControllerTests
    {
        private static IEnumerable<Category> InitialCategories => new[]
        {
            new Category { Id = 1, Description = "Testing", Code = "testing" },
            new Category { Id = 2, Description = "Testing", Code = "testing" },
        };

        private CategoryValidator TestValidator => new CategoryValidator();

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
        public void ShouldGetMultipleCategories()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
            var response = controller.Get();
            if (response.Value.Except(InitialCategories).Any())
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should post category.")]
        public void ShouldPostCategory()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
            var category = new Category
            {
                Description = "Testing",
                Code = "testing"
            };
            var response = controller.Post(category);
            category.Id = 3;
            if (response.Value.Equals(category))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not post category.")]
        public void ShouldNotPostCategory()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
            var category = new Category();
            var response = controller.Post(category);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Should update category.")]
        public void ShouldUpdateCategory()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
            var category = new Category
            {
                Id = 1,
                Description = "testing",
                Code = "Testing"
            };
            var response = controller.Put(category);
            if (response.Value.Equals(category))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not update category.")]
        public void ShouldNotUpdateCategory()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
            var category = new Category();
            var response = controller.Put(category);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Should delete category.")]
        public void ShouldDeleteCategory()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
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

        [Fact(DisplayName = "Should not delete category.")]
        public void ShouldNotDeleteCategory()
        {
            var controller = new CategoriesController(TestRepository, TestValidator);
            int id = 5;
            var response = controller.Delete(id);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
