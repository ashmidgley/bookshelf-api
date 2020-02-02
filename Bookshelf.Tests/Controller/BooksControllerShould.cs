using NUnit.Framework;
using Bookshelf.Core;
using System;
using System.Collections.Generic;
using FakeItEasy;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class BooksControllerShould
    {        
        private NewBookValidator _newBookValidator => new NewBookValidator();
        private UpdatedBookValidator _updatedBookValidator => new UpdatedBookValidator();

        [Test]
        public void ReturnAllBooks()
        {
            var result = new List<BookDto>();

            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetUserBooks(A<int>.Ignored)).Returns(result);

            var controller = new BooksController(repository, null, null, null, _newBookValidator, _updatedBookValidator);
            
            var books = controller.GetUserBooks(1);
            
            Assert.AreEqual(result, books);
        }

        [Test]
        public void AddNewBook()
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

            var result = new BookDto();

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var searchHelper = A.Fake<ISearchHelper>();
            A.CallTo(() => searchHelper.PullGoogleBooksData(A<NewBookDto>.Ignored)).Returns(new Book());

            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Add(A<Book>.Ignored)).Returns(1);
            A.CallTo(() => repository.GetBook(A<int>.Ignored)).Returns(result);

            var controller = new BooksController(repository, null, searchHelper, userHelper, _newBookValidator, _updatedBookValidator);

            var responseOne = controller.Post(newBook);
            var responseTwo = controller.Post(new NewBookDto());

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateExistingBook()
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

            var result = new BookDto();
            result.Id = 1;

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetBook(A<int>.Ignored)).Returns(result);
           
            var controller = new BooksController(repository, null, null, userHelper, _newBookValidator, _updatedBookValidator);

            var responseOne = controller.Put(updatedBook);
            var responseTwo = controller.Put(new BookDto());

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteBook()
        {
            const int id = 1;
            var result = new BookDto();
            result.Id = id;

            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetBook(id)).Returns(result);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var controller = new BooksController(repository, null, null, userHelper, _newBookValidator, _updatedBookValidator);

            var responseOne = controller.Delete(id);
            var responseTwo = controller.Delete(5);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }
    }
}