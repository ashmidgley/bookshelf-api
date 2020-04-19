using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Bookshelf.Core
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISearchHelper _searchHelper;
        private readonly IUserHelper _userHelper;
        private readonly NewBookValidator _newBookValidator;
        private readonly UpdatedBookValidator _updatedBookValidator;

        public BooksController(IBookRepository bookRepository, IUserRepository userRepository, ISearchHelper searchHelper, IUserHelper userHelper,
            NewBookValidator newBookValidator, UpdatedBookValidator updatedBookValidator)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _searchHelper = searchHelper;
            _userHelper = userHelper;
            _newBookValidator = newBookValidator;
            _updatedBookValidator = updatedBookValidator;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<BookDto> GetBook(int id)
        {
            if(!_bookRepository.BookExists(id))
            {
                return BadRequest($"Book with Id {id} does not exist.");
            }

            return _bookRepository.GetBook(id);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("user/{userId}")]
        public ActionResult<IEnumerable<BookDto>> GetUserBooks(int userId)
        {
            if(!_userRepository.UserExists(userId))
            {
                return BadRequest($"User with Id {userId} does not exist.");
            }

            return _bookRepository.GetUserBooks(userId).ToList();
        }

        [HttpPost]
        [Route("search")]
        public async Task<IEnumerable<Book>> SearchBooks([FromBody] SearchDto search)
        {
            return await _searchHelper.SearchBooks(search.Title, search.Author, search.MaxResults);
        } 

        [HttpPost]
        public ActionResult<BookDto> AddBook([FromBody] Book newBook)
        {
            var validation = _newBookValidator.Validate(newBook);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            
            if(!_userHelper.MatchingUsers(HttpContext, newBook.UserId))
            {
                return Unauthorized();
            }

            var id = _bookRepository.Add(newBook);
            return _bookRepository.GetBook(id);
        }

        [HttpPut]
        public ActionResult<BookDto> UpdateBook([FromBody] BookDto dto)
        {
            var validation = _updatedBookValidator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userHelper.MatchingUsers(HttpContext, dto.UserId))
            {
                return Unauthorized();
            }

            if(!_bookRepository.BookExists(dto.Id))
            {
                return BadRequest($"Book with Id {dto.Id} does not exist.");
            }

            _bookRepository.Update(dto);
            return _bookRepository.GetBook(dto.Id);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<BookDto> DeleteBook(int id)
        {
            if(!_bookRepository.BookExists(id))
            {
                return BadRequest($"Book with Id {id} does not exist.");
            }

            var book = _bookRepository.GetBook(id);
            if(!_userHelper.MatchingUsers(HttpContext, book.UserId))
            {
                return Unauthorized();
            }

            _bookRepository.Delete(id);
            return book;
        }
    }
}
