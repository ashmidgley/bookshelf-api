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
        public ActionResult<List<UserDto>> GetAll()
        {
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }
            
            return _userRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<UserDto> Get(int id)
        {
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }

            return _userRepository.GetUser(id);
        }

        [HttpPut]
        public ActionResult<UserDto> Update(UserDto user)
        {
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }

            var validation = _userDtoValidator.Validate(user);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userRepository.UserPresent(user.Id))
            {
                return BadRequest($"User with id {user.Id} not found.");
            }

            _userRepository.Update(user);

            return _userRepository.GetUser(user.Id);
        }

        [HttpPut]
        [Route("email")]
        public ActionResult<EmailUpdateDto> UpdateEmail(UserUpdateDto user)
        {
            if(!_userHelper.MatchingUsers(HttpContext, user.Id))
            {
               return Unauthorized(); 
            }
            
            if(_userRepository.UserPresent(user.Email))
            {
                return  new EmailUpdateDto
                {
                    Error = $"Email {user.Email} is already in use."
                };
            }

            var currentUser = _userRepository.GetUser(user.Id);
            currentUser.Email = user.Email;
            _userRepository.Update(currentUser);

            return new EmailUpdateDto
            {
                User = _userRepository.GetUser(user.Id)
            };
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
        public ActionResult<UserDto> Delete(int id)
        {
            var user = _userRepository.GetUser(id);
            if(user.Id == default)
            {
                return BadRequest($"Rating with id {user.Id} not found.");
            }
            
            if(!_userHelper.IsAdmin(HttpContext) && !_userHelper.MatchingUsers(HttpContext, id))
            {
                return Unauthorized();
            }

            _userHelper.DeleteUser(id);

            return user;
        }
    }
}
