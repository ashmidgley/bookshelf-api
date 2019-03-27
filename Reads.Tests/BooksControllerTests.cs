using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Reads.Controllers;
using Reads.Models;
using Xunit;

namespace Reads.Tests
{
    public class BooksControllerTests
    {
        private static IEnumerable<Book> InitialBooks => new[]
        {
            new Book { Id = 1, Image = "testing", CategoryId = 1, StartedOn = new DateTime(2019,1,1), FinishedOn = new DateTime(2019,1,11), PageCount = 112, Title = "Testing", Author = "Testing", Summary = "Testing", Removed = false },
            new Book { Id = 2, Image = "testing", CategoryId = 1, StartedOn = new DateTime(2019,1,1), FinishedOn = new DateTime(2019,1,11), PageCount = 112, Title = "Testing", Author = "Testing", Summary = "Testing", Removed = false }
        };

        private static IEfRepository<Book> TestRepository
        {
            get
            {
                var dbContextMock = new DbContextMock<ReadsContext>();
                dbContextMock.CreateDbSetMock(x => x.Books, InitialBooks);
                return new EfRepository<Book>(dbContextMock.Object);
            }
        }

        [Fact(DisplayName = "Should get multiple books.")]
        public void ShouldGetMultipleBooks()
        {
            var controller = new BooksController(TestRepository);
            var response = controller.Get();
            var books = response.Value;
            books.Should().HaveCount(InitialBooks.Count());
        }

        [Fact(DisplayName = "Should post book.")]
        public void ShouldPostBook()
        {
            var controller = new BooksController(TestRepository);
            var book = new Book
            {
                Image = "testing",
                CategoryId = 1,
                StartedOn = new DateTime(2019, 1, 1),
                FinishedOn = new DateTime(2019, 1, 11),
                PageCount = 112,
                Title = "Testing",
                Author = "Testing",
                Summary = "Testing"
            };
            var response = controller.Post(book);
            ((StatusCodeResult)response).StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        /// WIP - Need to add Fluent Assertions and remove DataAnnotations for model checking
        //[Fact(DisplayName = "Should not post book.")]
        //public void ShouldNotPostBook()
        //{
        //    var controller = new BooksController(TestRepository);
        //    var book = new Book();
        //    var response = controller.Post(book);
        //    ((BadRequestObjectResult)response).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        //}

        [Fact(DisplayName = "Should delete book.")]
        public void ShouldDeleteBook()
        {
            var controller = new BooksController(TestRepository);
            int id = 1;
            var response = controller.Delete(id);
            ((StatusCodeResult) response).StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Should not delete book.")]
        public void ShouldNotDeleteBook()
        {
            var controller = new BooksController(TestRepository);
            int id = 5;
            var response = controller.Delete(id);
            ((BadRequestObjectResult) response).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
