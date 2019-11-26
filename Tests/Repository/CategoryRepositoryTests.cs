using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class CategoryRepositoryTests
    {
        DbContextOptions<BookshelfContext> options;

        public CategoryRepositoryTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(databaseName: "Bookshelf")
                .Options;

            using (var context = new BookshelfContext(options))
            {
                context.Categories.Add(new Category { Id = 1, Description = "Fiction", Code = "🧟" });
                context.Categories.Add(new Category { Id = 2, Description = "Non-fiction", Code = "🧠" });
                context.SaveChanges();
            }

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(context.Categories.CountAsync().Result, 2);
            }
        }

        [Test]
        public void GetAllTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                Assert.AreEqual(repository.GetAll().Result.Count, 2);
            }
        }

        [Test]
        public void GetSingleTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new CategoryRepository(context);
                Assert.NotNull(repository.Get(1).Result);
                Assert.Null(repository.Get(5).Result);
            }
        }
    }
}