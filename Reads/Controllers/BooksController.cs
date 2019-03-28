using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reads.Models;
using Reads.Validators;

namespace Reads.Controllers
{
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
            List<Book> books = _bookRepository.GetAll().Result;
            return books;
        }

        // POST api/books
        [HttpPost]
        public ActionResult Post([FromBody] Book book)
        {
            var validation = _validator.Validate(book);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _bookRepository.Add(book);
            return Ok();
        }

        // PUT api/books
        [HttpPut]
        public ActionResult Put([FromBody] Book book)
        {
            var validation = _validator.Validate(book);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _bookRepository.Update(book);
            return Ok();
        }

        // DELETE api/books
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = _bookRepository.Get(id).Result;
            if (book == null)
            {
                return BadRequest($"No book found for id: {id}");
            }
            _bookRepository.Delete(book);
            return Ok();
        }
    }
}
