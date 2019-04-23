using System;
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
    public class BooksControllerTests
    {
        private static IEnumerable<Book> InitialBooks => new[]
        {
            new Book { Id = 1, Image = "testing", CategoryId = 1, StartedOn = new DateTime(2019,1,1), FinishedOn = new DateTime(2019,1,11), PageCount = 112, Title = "Testing", Author = "Testing", Summary = "Testing", Removed = false },
            new Book { Id = 2, Image = "testing", CategoryId = 1, StartedOn = new DateTime(2019,1,1), FinishedOn = new DateTime(2019,1,11), PageCount = 112, Title = "Testing", Author = "Testing", Summary = "Testing", Removed = false }
        };

        private BookValidator TestValidator => new BookValidator();

        private IBookRepository TestRepository
        {
            get
            {
                var dbContextMock = new DbContextMock<BookshelfContext>();
                dbContextMock.CreateDbSetMock(x => x.Books, InitialBooks);
                return new BookRepository(dbContextMock.Object);
            }
        }

        [Fact(DisplayName = "Should get multiple books.")]
        public void ShouldGetMultipleBooks()
        {
            var controller = new BooksController(TestRepository, TestValidator);
            var response = controller.Get();
            if (response.Value.Except(InitialBooks).Any())
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should post book.")]
        public void ShouldPostBook()
        {
            var controller = new BooksController(TestRepository, TestValidator);
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
            book.Id = 3;
            if (response.Value.Equals(book))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not post book.")]
        public void ShouldNotPostBook()
        {
            var controller = new BooksController(TestRepository, TestValidator);
            var book = new Book();
            var response = controller.Post(book);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Should update book.")]
        public void ShouldUpdateBook()
        {
            var controller = new BooksController(TestRepository, TestValidator);
            var book = new Book
            {
                Id = 1,
                Image = "updated",
                CategoryId = 1,
                StartedOn = new DateTime(2019, 2, 1),
                FinishedOn = new DateTime(2020, 2, 11),
                PageCount = 112,
                Title = "Updated",
                Author = "Updated",
                Summary = "Updated"
            };
            var response = controller.Put(book);
            if (response.Value.Equals(book))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not update book.")]
        public void ShouldNotUpdateBook()
        {
            var controller = new BooksController(TestRepository, TestValidator);
            var book = new Book();
            var response = controller.Put(book);
            ((BadRequestObjectResult)response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Should delete book.")]
        public void ShouldDeleteBook()
        {
            var controller = new BooksController(TestRepository, TestValidator);
            const int id = 1;
            var response = controller.Delete(id);
            if (response.Value.Id == id)
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, "Response does not match expected value.");
            }
        }

        [Fact(DisplayName = "Should not delete book.")]
        public void ShouldNotDeleteBook()
        {
            var controller = new BooksController(TestRepository, TestValidator);
            int id = 5;
            var response = controller.Delete(id);
            ((BadRequestObjectResult) response.Result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
