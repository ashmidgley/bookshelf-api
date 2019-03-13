using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reads.Models;

namespace Reads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            IEnumerable<Book> books = _bookRepository.GetAll();
            return Ok(books);
        }

        // POST api/books
        [HttpPost]
        public ActionResult Post([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _bookRepository.Add(book);
            return Ok();
        }

        // PUT api/books
        [HttpPut]
        public ActionResult Put([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
            var book = _bookRepository.Get(id);
            _bookRepository.Delete(book);
            return Ok();
        }
    }
}
