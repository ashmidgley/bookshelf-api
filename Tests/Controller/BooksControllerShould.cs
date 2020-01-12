using NUnit.Framework;
using Api;
using System;
using System.Collections.Generic;
using FakeItEasy;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class BooksControllerShould
    {        
        private BookValidator BookValidator => new BookValidator();
        private BookDtoValidator DtoValidator => new BookDtoValidator();
        private readonly List<BookDto> TestBooks = new List<BookDto>
        {
            new BookDto
            { 
                Id = 1,
                UserId = 1,
                ImageUrl = "fight-club.png",
                CategoryId = 1,
                FinishedOn = new DateTime(2019,4,12),
                Year = 2019,
                PageCount = 224,
                Title = "Fight Club",
                Author = "Chuck Palahniuk",
                Summary = "Chuck Palahniuk showed himself to be his generation’s most visionary satirist in this, his first book..."
            },
            new BookDto
            {
                Id = 2,
                UserId = 1,
                ImageUrl = "choke.png",
                CategoryId = 1,
                FinishedOn = new DateTime(2019,9,27),
                Year = 2019,
                PageCount = 293,
                Title = "Choke",
                Author = "Chuck Palahniuk",
                Summary = "Victor Mancini, a medical-school dropout, is an antihero for our deranged times..."
            }
        };
        private readonly BookDto BookSuccess = new BookDto
        {
            Id = 1,
            ImageUrl = "fight-club.png",
            CategoryId = 2,
            FinishedOn = new DateTime(2019,4,12),
            Year = 2019,
            PageCount = 500,
            Title = "Fight Club",
            Author = "Chucky Pal",
            Summary = "Updated summary..."
        };
        private readonly BookDto BookFail = new BookDto();

        [Test]
        public void ReturnAllBooks()
        {
            const int userId = 1;
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetUserBooks(userId)).Returns(TestBooks);
            var controller = new BooksController(repository, BookValidator, DtoValidator);
            
            var books = controller.GetUserBooks(userId);
            
            Assert.AreEqual(TestBooks, books);
        }

        [Test]
        public void AddNewBook()
        {
            var bookSuccess = new Book
            {
                ImageUrl = "fight-club.png",
                CategoryId = 2,
                FinishedOn = new DateTime(2019,4,12),
                PageCount = 500,
                Title = "Fight Club",
                Author = "Chucky Pal",
                Summary = "Updated summary..."
            };
            var bookFail = new Book();
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Add(bookSuccess)).Returns(BookSuccess.Id);
            A.CallTo(() => repository.GetBook(BookSuccess.Id)).Returns(BookSuccess);
            var controller = new BooksController(repository, BookValidator, DtoValidator);

            var responseOne = controller.Post(bookSuccess);
            var responseTwo = controller.Post(bookFail);

            Assert.AreEqual(BookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateExistingBook()
        {
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetBook(BookSuccess.Id)).Returns(BookSuccess);
            var controller = new BooksController(repository, BookValidator, DtoValidator);

            var responseOne = controller.Put(BookSuccess);
            var responseTwo = controller.Put(BookFail);

            Assert.AreEqual(BookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteBook()
        {
            var result = BookSuccess;
            result.Id = 1;
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetBook(A<int>.Ignored)).Returns(result);
            var controller = new BooksController(repository, BookValidator, DtoValidator);

            var responseOne = controller.Delete(BookSuccess);
            var responseTwo = controller.Delete(BookFail);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }
    }
}