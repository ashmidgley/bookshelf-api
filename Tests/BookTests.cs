using NUnit.Framework;
using Api;
using System;
using System.Collections.Generic;
using FakeItEasy;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class BookTests
    {        
        private BookValidator Validator => new BookValidator();
        private readonly List<Book> TestBooks = new List<Book>
        {
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
            },
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
        };
        private readonly Book TestBook = new Book
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

        [Test]
        public void GetAllTest()
        {
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.GetAll()).Returns(TestBooks);
            var controller = new BooksController(repository, Validator);
            
            var books = controller.Get().Value;
            
            Assert.AreEqual(TestBooks, books);
        }

        [Test]
        public void PostTest()
        {
            const int id = 1;
            var bookSuccess = TestBook;
            var result = TestBook;
            result.Id = id;
            var bookFail = new Book();
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Add(bookSuccess)).Returns(id);
            A.CallTo(() => repository.Get(id)).Returns(result);
            var controller = new BooksController(repository, Validator);

            var responseOne = controller.Post(bookSuccess);
            bookSuccess.Id = id;
            var responseTwo = controller.Post(bookFail);

            Assert.AreEqual(bookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateTest()
        {
            const int id = 1;
            var bookSuccess = TestBook;
            bookSuccess.Id = id;
            bookSuccess.Summary = "Updated summary...";
            var bookFail = new Book();
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Get(id)).Returns(bookSuccess);
            var controller = new BooksController(repository, Validator);

            var responseOne = controller.Put(bookSuccess);
            var responseTwo = controller.Put(bookFail);

            Assert.AreEqual(bookSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteTest()
        {
            const int idSuccess = 1;
            var result = TestBook;
            result.Id = idSuccess;
            const int idFail = 5;
            var repository = A.Fake<IBookRepository>();
            A.CallTo(() => repository.Get(idSuccess)).Returns(result);
            A.CallTo(() => repository.Get(idFail)).Returns(new Book());
            var controller = new BooksController(repository, Validator);
            
            var responseOne = controller.Delete(idSuccess);
            var responseTwo = controller.Delete(idFail);

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            Assert.AreEqual($"Book with id {idFail} not found.", ((BadRequestObjectResult)responseTwo.Result).Value);
        }
    }
}