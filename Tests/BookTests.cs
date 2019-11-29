using NUnit.Framework;
using Bookshelf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class BookTests
    {
        private readonly DbContextOptions<BookshelfContext> options;
        private BookValidator Validator => new BookValidator();

        public BookTests()
        {
            options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            using (var context = new BookshelfContext(options))
            {
                context.Books.Add(
                    new Book 
                    { 
                        Image = "fight-club.png",
                        CategoryId = 1,
                        StartedOn = new DateTime(2019,1,1),
                        FinishedOn = new DateTime(2019,4,12),
                        PageCount = 224,
                        Title = "Fight Club",
                        Author = "Chuck Palahniuk",
                        Summary = "Chuck Palahniuk showed himself to be his generation’s most visionary satirist in this, his first book..."
                    }
                );
                context.Books.Add(
                    new Book
                    {
                        Image = "choke.png",
                        CategoryId = 1,
                        StartedOn = new DateTime(2019,5,5),
                        FinishedOn = new DateTime(2019,9,27),
                        PageCount = 293,
                        Title = "Choke",
                        Author = "Chuck Palahniuk",
                        Summary = "Victor Mancini, a medical-school dropout, is an antihero for our deranged times..."
                    }
                );
                context.SaveChanges();
            };

            using (var context = new BookshelfContext(options))
            {
                Assert.AreEqual(2, context.Books.CountAsync().Result);
            }
        }

        [Test]
        [Order(1)]
        public void GetAllTest()
        {
            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                var controller = new BooksController(repository, Validator);
                var books = controller.Get().Value.ToList();
                Assert.AreEqual(2, books.Count);
            }
        }

        [Test]
        public void PostTest()
        {
            var bookSuccess = new Book
            {
                Image = "testing.png",
                CategoryId = 1,
                StartedOn = new DateTime(2019, 1, 1),
                FinishedOn = new DateTime(2019, 1, 11),
                PageCount = 112,
                Title = "Testing",
                Author = "Testing",
                Summary = "Testing"
            };
            var bookFail = new Book();

            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                var controller = new BooksController(repository, Validator);

                var responseOne = controller.Post(bookSuccess);
                bookSuccess.Id = 3;
                Assert.AreEqual(bookSuccess, responseOne.Value);

                var responseTwo = controller.Post(bookFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            }
        }

        [Test]
        public void UpdateTest()
        {
            var bookSuccess = new Book
            {
                Id = 1,
                Image = "fight-club.png",
                CategoryId = 2,
                StartedOn = new DateTime(2019,1,1),
                FinishedOn = new DateTime(2019,4,12),
                PageCount = 500,
                Title = "Fight Club",
                Author = "Chucky Pal",
                Summary = "Updated summary..."
            };
            var bookFail = new Book();

            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                var controller = new BooksController(repository, Validator);

                var responseOne = controller.Put(bookSuccess);
                Assert.AreEqual(bookSuccess, responseOne.Value);

                var responseTwo = controller.Put(bookFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            }
        }

        [Test]
        public void DeleteTest()
        {
            int idSuccess = 2;
            int idFail = 5;

            using (var context = new BookshelfContext(options))
            {
                var repository = new BookRepository(context);
                var controller = new BooksController(repository, Validator);

                var responseOne = controller.Delete(idSuccess);
                Assert.AreEqual(idSuccess, responseOne.Value.Id);
                
                var responseTwo = controller.Delete(idFail);
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            }
        }
    }
}