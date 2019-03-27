using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reads.Models;

namespace Reads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IEfRepository<Book> _bookRepository;

        public BooksController(IEfRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
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
