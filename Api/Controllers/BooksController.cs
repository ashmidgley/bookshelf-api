using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookValidator _bookValidator;
        private readonly BookDtoValidator _dtoValidator;

        public BooksController(IBookRepository bookRepository, BookValidator bookValidator, BookDtoValidator dtoValidator)
        {
            _bookRepository = bookRepository;
            _bookValidator = bookValidator;
            _dtoValidator = dtoValidator;
        }

        // GET api/books
        [HttpGet]
        public IEnumerable<BookDto> Get()
        {
            return _bookRepository.GetAll();
        }

        // POST api/books
        [HttpPost]
        public ActionResult<BookDto> Post([FromBody] Book book)
        {
            var validation = _bookValidator.Validate(book);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            var id = _bookRepository.Add(book);
            return _bookRepository.Get(id);
        }

        // PUT api/books
        [HttpPut]
        public ActionResult<BookDto> Put([FromBody] BookDto dto)
        {
            var validation = _dtoValidator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _bookRepository.Update(dto);
            return _bookRepository.Get(dto.Id);
        }

        // DELETE api/books
        [HttpDelete("{id}")]
        public ActionResult<BookDto> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dto = _bookRepository.Get(id);
            if (dto.Id == default)
            {
                return BadRequest($"Book with id {id} not found.");
            }
            _bookRepository.Delete(dto.Id);
            return dto;
        }
    }
}
