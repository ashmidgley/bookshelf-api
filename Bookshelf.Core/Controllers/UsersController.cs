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
        private readonly LoginDtoValidator _loginValidator;
        private readonly UserDtoValidator _userValidator;

        public UsersController(IUserRepository userRepository, IUserHelper userHelper, LoginDtoValidator loginValidator,
            UserDtoValidator userValidator)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
            _loginValidator = loginValidator;
            _userValidator = userValidator;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult<TokenDto> Login([FromBody]LoginDto login)
        {
            var validation = _loginValidator.Validate(login);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if (!_userRepository.Authenticate(login))
            {
                return new TokenDto
                {
                    Error = "Incorrect credentials. Please try again."
                };
            }

            var user = _userRepository.GetUser(login.Email);

            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(user)
            };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult<TokenDto> Register(LoginDto login)
        {
            var validation = _loginValidator.Validate(login);

            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(_userRepository.UserPresent(login.Email)) 
            {
                return new TokenDto
                {
                    Error = "Email already in use. Please try another."
                };
            }

            var id = _userRepository.Add(login);
            _userHelper.Register(id);
            var user = _userRepository.GetUser(id);

            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(user)
            };
        }

        [HttpPut]
        public ActionResult<UserDto> Update(UserDto user)
        {
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }

            var validation = _userValidator.Validate(user);
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
            
            if(!_userHelper.IsAdmin(HttpContext))
            {
                return Unauthorized();
            }

            _userRepository.Delete(id);

            return user;
        }
    }
}
