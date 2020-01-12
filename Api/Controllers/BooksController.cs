using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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

        [HttpGet]
        [Route("{bookId}")]
        public ActionResult<BookDto> GetBook(int bookId)
        {
            return _bookRepository.GetBook(bookId);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("user/{userId}")]
        public IEnumerable<BookDto> GetUserBooks(int userId)
        {
            return _bookRepository.GetUserBooks(userId);
        }

        [HttpPost]
        public ActionResult<BookDto> Post([FromBody] Book book)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != book.UserId)
            {
                return Unauthorized();
            }

            var validation = _bookValidator.Validate(book);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var id = _bookRepository.Add(book);
            
            return _bookRepository.GetBook(id);
        }

        [HttpPut]
        public ActionResult<BookDto> Put([FromBody] BookDto dto)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != dto.UserId)
            {
                return Unauthorized();
            }

            var validation = _dtoValidator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            
            var current = _bookRepository.GetBook(dto.Id);
            if(current.Id == default)
            {
                return BadRequest($"Book with id {current.Id} not found.");
            }

            _bookRepository.Update(dto);
            
            return _bookRepository.GetBook(dto.Id);
        }

        [HttpDelete]
        public ActionResult<BookDto> Delete([FromBody] BookDto dto)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != dto.UserId)
            {
                return Unauthorized();
            }

            var validation = _dtoValidator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var current = _bookRepository.GetBook(dto.Id);
            if(current.Id == default)
            {
                return BadRequest($"Book with id {current.Id} not found.");
            }

            _bookRepository.Delete(dto.Id);
            
            return current;
        }
    }
}
