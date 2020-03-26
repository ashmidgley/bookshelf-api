using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshelf.Core
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly UserDtoValidator _userDtoValidator;

        public UsersController(IUserRepository userRepository, IUserHelper userHelper, UserDtoValidator userDtoValidator)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
            _userDtoValidator = userDtoValidator;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<UserDto> GetUser(int id)
        {
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }

            return _userRepository.GetUser(id);
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers()
        {
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }
            
            return _userRepository.GetAll().ToList();
        }

        [HttpPut]
        public ActionResult<UserDto> UpdateUser(UserDto user)
        {
            var validation = _userDtoValidator.Validate(user);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }

            if(!_userRepository.UserPresent(user.Id))
            {
                return BadRequest($"User with Id {user.Id} does not exist.");
            }

            _userRepository.Update(user);
            return _userRepository.GetUser(user.Id);
        }

        [HttpPut]
        [Route("email")]
        public ActionResult<UserDto> UpdateEmail(UserUpdateDto user)
        {
            if(!_userHelper.MatchingUsers(HttpContext, user.Id))
            {
               return Unauthorized(); 
            }
            
            if(_userRepository.UserPresent(user.Email))
            {
                return BadRequest($"Email {user.Email} is already in use.");
            }

            var currentUser = _userRepository.GetUser(user.Id);
            currentUser.Email = user.Email;
            _userRepository.Update(currentUser);
            return _userRepository.GetUser(user.Id);
        }
        
        [HttpPut]
        [Route("password")]
        public ActionResult<UserDto> UpdatePassword(UserUpdateDto user)
        {
            if(!_userHelper.MatchingUsers(HttpContext, user.Id))
            {
               return Unauthorized(); 
            }

            var passwordHash = _userHelper.HashPassword(user.Password);
            _userRepository.UpdatePasswordHash(user.Id, passwordHash);
            return _userRepository.GetUser(user.Id);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<UserDto> DeleteUser(int id)
        {
            if(!_userRepository.UserPresent(id))
            {
                return BadRequest($"User with Id {id} does not exist.");
            }
            
            if(!_userHelper.IsAdmin(HttpContext) && !_userHelper.MatchingUsers(HttpContext, id))
            {
                return Unauthorized();
            }

            _userHelper.DeleteUser(id);
            return _userRepository.GetUser(id);
        }
    }
}
