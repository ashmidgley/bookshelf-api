using NUnit.Framework;
using Bookshelf.Core;
using System;
using System.Collections.Generic;
using FakeItEasy;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class BooksControllerShould
    {
        [Test]
        public void ReturnGetBook_OnCallToGetBook()
        {
            var bookRepository = A.Fake<IBookRepository>();
            var controller = new BooksController(bookRepository, null, null, null, null);
            
            var response = controller.GetBook(1);
            
            A.CallTo(() => bookRepository.GetBook(1)).MustHaveHappened();
        }

        [Test]
        public void ReturnGetUserBooks_OnCallToGetUserBooks()
        {
            var bookRepository = A.Fake<IBookRepository>();
            var controller = new BooksController(bookRepository, null, null, null, null);
            
            var response = controller.GetUserBooks(1);
            
            A.CallTo(() => bookRepository.GetUserBooks(1)).MustHaveHappened();
        }

        [Test]
        public async Task ReturnBookDto_WhenValidUser_CallsAddBook()
        {
            var newBook = new NewBookDto
            {
                Title = "Test",
                Author = "Test",
                UserId = 1,
                CategoryId = 2,
                RatingId = 1,
                FinishedOn = DateTime.Now
            };

            var result = new BookDto
            {
                UserId = newBook.UserId
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, newBook.UserId)).Returns(true);

            var searchHelper = A.Fake<ISearchHelper>();

            var bookRepository = A.Fake<IBookRepository>();
            A.CallTo(() => bookRepository.GetBook(A<int>.Ignored)).Returns(result);

            var newBookValidator = new NewBookValidator();

            var controller = new BooksController(bookRepository, searchHelper, userHelper, newBookValidator, null);

            var response = await controller.AddBook(newBook);

            A.CallTo(() => searchHelper.PullGoogleBooksData(newBook)).MustHaveHappened();
            A.CallTo(() => bookRepository.Add(A<Book>.Ignored)).MustHaveHappened();
            Assert.AreEqual(result.UserId, response.Value.UserId);
        }

        [Test]
        public void ReturnBookDto_WhenValidUser_CallsUpdateBook()
        {
            var updatedBook = new BookDto
            {
                Id = 1,
                UserId = 1,
                CategoryId = 2,
                RatingId = 1,
                Title = "Test",
                Author = "Test",
                FinishedOn = DateTime.Now,
                ImageUrl = "test.png",
                Year = 2019,
                PageCount = 111,
                Summary = "test"
            };

            var result = new BookDto
            {
                Id = updatedBook.Id
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedBook.UserId)).Returns(true);

            var bookRepository = A.Fake<IBookRepository>();
            A.CallTo(() => bookRepository.BookExists(updatedBook.Id)).Returns(true);
            A.CallTo(() => bookRepository.GetBook(updatedBook.Id)).Returns(result);
           
            var updatedBookValidator = new UpdatedBookValidator();

            var controller = new BooksController(bookRepository, null, userHelper, null, updatedBookValidator);

            var response = controller.UpdateBook(updatedBook);

            A.CallTo(() => bookRepository.Update(A<BookDto>.Ignored)).MustHaveHappened();
            Assert.AreEqual(result.Id, response.Value.Id);
        }

        [Test]
        public void ReturnBookDto_WhenValidUser_CallsDeleteBook()
        {
            var id = 1;
            
            var result = new BookDto
            {
                Id = id,
                UserId = 1
            };

            var bookRepository = A.Fake<IBookRepository>();
            A.CallTo(() => bookRepository.BookExists(id)).Returns(true);
            A.CallTo(() => bookRepository.GetBook(id)).Returns(result);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, result.UserId)).Returns(true);

            var controller = new BooksController(bookRepository, null, userHelper, null, null);

            var response = controller.DeleteBook(id);
            
            A.CallTo(() => bookRepository.Delete(id)).MustHaveHappened();
            Assert.AreEqual(result.Id, response.Value.Id);
        }
    }
}