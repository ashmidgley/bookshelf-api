using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Bookshelf.Core
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookHelper _bookHelper;
        private readonly ISearchHelper _searchHelper;
        private readonly IUserHelper _userHelper;
        private readonly NewBookValidator _newBookValidator;
        private readonly UpdatedBookValidator _updatedBookValidator;

        public BooksController(IBookRepository bookRepository, IBookHelper bookHelper, ISearchHelper searchHelper, IUserHelper userHelper,
            NewBookValidator newBookValidator, UpdatedBookValidator updatedBookValidator)
        {
            _bookRepository = bookRepository;
            _bookHelper = bookHelper;
            _searchHelper = searchHelper;
            _userHelper = userHelper;
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
        public async Task<ActionResult<BookDto>> Post([FromBody] NewBookDto newBook)
        {
            if(!_userHelper.MatchingUsers(HttpContext, newBook.UserId))
            {
                return Unauthorized();
            }

            var validation = _newBookValidator.Validate(newBook);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var book = await _searchHelper.PullGoogleBooksData(newBook);
            var id = _bookRepository.Add(book);

            return _bookRepository.GetBook(id);
        }

        [HttpPut]
        public ActionResult<BookDto> Put([FromBody] BookDto dto)
        {
            if(!_userHelper.MatchingUsers(HttpContext, dto.UserId))
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
            
            if(!_userHelper.MatchingUsers(HttpContext, book.UserId))
            {
                return Unauthorized();
            }

            _bookRepository.Delete(id);
            
            return book;
        }
    }
}
