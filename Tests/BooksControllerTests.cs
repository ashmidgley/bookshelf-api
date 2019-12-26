using NUnit.Framework;
using Api;
using System;
using System.Collections.Generic;
using FakeItEasy;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class BooksControllerTests
    {        
        private BookValidator BookValidator => new BookValidator();
        private BookDtoValidator DtoValidator => new BookDtoValidator();
        private readonly List<BookDto> TestBooks = new List<BookDto>
        {
            new BookDto
            { 
                Id = 1,
                Image = "fight-club.png",
                CategoryId = 1,
                StartedOn = new DateTime(2019,1,1),
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
                Image = "choke.png",
                CategoryId = 1,
                StartedOn = new DateTime(2019,5,5),
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
            Image = "fight-club.png",
            CategoryId = 2,
            StartedOn = new DateTime(2019,1,1),
            FinishedOn = new DateTime(2019,4,12),
            Year = 2019,
            PageCount = 500,
            Title = "Fight Club",
            Author = "Chucky Pal",
            Summary = "Updated summary..."
        };
        private readonly BookDto BookFail = new BookDto();

        [Test]
        public void GetAllTest()
        {
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetAll()).Returns(TestBooks);
            var controller = new BooksController(repository, BookValidator, DtoValidator);
            
            var books = controller.GetAll();
            
            Assert.AreEqual(TestBooks, books);
        }

        [Test]
        public void PostTest()
        {
            var bookSuccess = new Book
            {
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
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Add(bookSuccess)).Returns(BookSuccess.Id);
            A.CallTo(() => repository.Get(BookSuccess.Id)).Returns(BookSuccess);
            var controller = new BooksController(repository, BookValidator, DtoValidator);

            var responseOne = controller.Post(bookSuccess);
            var responseTwo = controller.Post(bookFail);

            Assert.AreEqual(BookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateTest()
        {
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Get(BookSuccess.Id)).Returns(BookSuccess);
            var controller = new BooksController(repository, BookValidator, DtoValidator);

            var responseOne = controller.Put(BookSuccess);
            var responseTwo = controller.Put(BookFail);

            Assert.AreEqual(BookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteTest()
        {
            const int idFail = 5;
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Get(BookSuccess.Id)).Returns(BookSuccess);
            var controller = new BooksController(repository, BookValidator, DtoValidator);
            
            var responseOne = controller.Delete(BookSuccess.Id);
            var responseTwo = controller.Delete(idFail);

            Assert.AreEqual(BookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            Assert.AreEqual($"Book with id {idFail} not found.", ((BadRequestObjectResult)responseTwo.Result).Value);
        }
    }
}