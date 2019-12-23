using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Bookshelf
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookValidator _validator;

        public BooksController(IBookRepository bookRepository, BookValidator validator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
        }

        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            return _bookRepository.GetAll().Result;
        }

        // POST api/books
        [HttpPost]
        public ActionResult<Book> Post([FromBody] Book book)
        {
            var validation = _validator.Validate(book);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            var id = _bookRepository.Add(book).Result;
            return _bookRepository.Get(id).Result;
        }

        // PUT api/books
        [HttpPut]
        public ActionResult<Book> Put([FromBody] Book book)
        {
            var validation = _validator.Validate(book);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _bookRepository.Update(book);
            return _bookRepository.Get(book.Id).Result;
        }

        // DELETE api/books
        [HttpDelete("{id}")]
        public ActionResult<Book> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = _bookRepository.Get(id).Result;
            if (book.Id == default)
            {
                return BadRequest($"Book with id {id} not found.");
            }
            _bookRepository.Delete(book);
            return book;
        }
    }
}
