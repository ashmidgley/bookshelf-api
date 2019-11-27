using NUnit.Framework;
using Api;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class CategoryTests
    {
        private readonly DbContextOptions<BookshelfContext> options;
        private CategoryValidator Validator => new CategoryValidator();

        public CategoryTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new BookshelfContext(options))
            {
                context.Categories.Add(new Category { Description = "Fiction", Code = "🧟" });
                context.Categories.Add(new Category { Description = "Non-fiction", Code = "🧠" });
                context.SaveChanges();
            }

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(2, context.Categories.CountAsync().Result);
            }
        }

        [Test]
        public void GetAllTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                var controller = new CategoriesController(repository, Validator);
                var Categories = controller.Get().Value.ToList();
                Assert.AreEqual(2, Categories.Count);
            }
        }

        [Test]
        public void PostTest()
        {
            var categorySuccess = new Category { Description = "Sci-fi", Code = "🚀" };
            var categoryFail = new Category();
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                var controller = new CategoriesController(repository, Validator);
                var responseOne = controller.Post(categorySuccess);
                categorySuccess.Id = 3;
                Assert.AreEqual(categorySuccess, responseOne.Value);
                var responseTwo = controller.Post(categoryFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
                Assert.IsNull(responseTwo.Value);
            }
        }

        [Test]
        public void UpdateTest()
        {
            var categorySuccess = new Category { Id = 1, Description = "Fiction", Code = "🧟" };
            var categoryFail = new Category();
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                var controller = new CategoriesController(repository, Validator);
                var responseOne = controller.Put(categorySuccess);
                Assert.AreEqual(categorySuccess, responseOne.Value);
                var responseTwo = controller.Put(categoryFail);
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
                var repository = new CategoryRepository(context);
                var controller = new CategoriesController(repository, Validator);
                var responseOne = controller.Delete(idSuccess);
                Assert.AreEqual(idSuccess, responseOne.Value.Id);
                var responseTwo = controller.Delete(idFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
                Assert.IsNull(responseTwo.Value);
            }
            var category = new Category { Description = "Non-fiction", Code = "🧠" };
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                var controller = new CategoriesController(repository, Validator);
                controller.Post(category);
            }
        }
    }
}