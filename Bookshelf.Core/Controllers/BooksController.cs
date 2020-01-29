using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Bookshelf.Core
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookHelper _bookHelper;
        private readonly NewBookValidator _newBookValidator;
        private readonly UpdatedBookValidator _updatedBookValidator;

        public BooksController(IBookRepository bookRepository, IBookHelper bookHelper, NewBookValidator newBookValidator,
            UpdatedBookValidator updatedBookValidator)
        {
            _bookRepository = bookRepository;
            _bookHelper = bookHelper;
            _newBookValidator = newBookValidator;
            _updatedBookValidator = updatedBookValidator;
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
        public ActionResult<BookDto> Post([FromBody] NewBookDto newBook)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != newBook.UserId)
            {
                return Unauthorized();
            }

            var validation = _newBookValidator.Validate(newBook);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var book = _bookHelper.PullOpenLibraryData(newBook).Result;
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

            var validation = _updatedBookValidator.Validate(dto);
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

        [HttpDelete("{id}")]
        public ActionResult<BookDto> Delete(int id)
        {
            var book = _bookRepository.GetBook(id);
            if(book.Id == default)
            {
                return BadRequest($"Book with id {book.Id} not found.");
            }

            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);
            
            if(userId != book.UserId)
            {
                return Unauthorized();
            }

            _bookRepository.Delete(id);
            
            return book;
        }
    }
}
