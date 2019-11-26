using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Tests
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<BookshelfContext> options;
        private CategoryValidator Validator => new CategoryValidator();

        public CategoryRepositoryTests()
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

        /* Repository tests. */

        [Test]
        public void GetAllRepositoryTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                Assert.AreEqual(2, repository.GetAll().Result.Count);
            }
        }

        [Test]
        public void GetSingleRepositoryTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                Assert.NotNull(repository.Get(1).Result);
                Assert.Null(repository.Get(5).Result);
            }
        }

        /* Controller Tests. */

        [Test]
        public void GetAllControllerTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                var controller = new CategoriesController(repository, Validator);
                var Categories = controller.Get().Value.ToList();
                Assert.AreEqual(2, Categories.Count);
            }
        }
    }
}