using System;
using Microsoft.AspNetCore.Mvc;

namespace Bookshelf.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IUserHelper _userHelper;
        private LoginDtoValidator _loginDtoValidator;

        public AuthController(IUserRepository userRepository, IUserHelper userHelper, LoginDtoValidator loginDtoValidator)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
            _loginDtoValidator = loginDtoValidator;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<TokenDto> Login([FromBody]LoginDto login)
        {
            var validation = _loginDtoValidator.Validate(login);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userRepository.UserPresent(login.Email))
            {
                return new TokenDto
                {
                    Error = "Incorrect email address. Please try again."
                };
            }

            if (!_userHelper.PasswordsMatch(login.Password, _userRepository.GetPasswordHash(login.Email)))
            {
                return new TokenDto
                {
                    Error = "Incorrect password. Please try again."
                };
            }

            var user = _userRepository.GetUser(login.Email);

            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(user)
            };
        }

        [HttpPost]
        [Route("register")]
        public ActionResult<TokenDto> Register(LoginDto login)
        {
            var validation = _loginDtoValidator.Validate(login);

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

            var newUser = new User
            {
                Email = login.Email,
                PasswordHash = _userHelper.HashPassword(login.Password),
            };

            var id = _userRepository.Add(newUser);
            _userHelper.Register(id);
            var user = _userRepository.GetUser(id);

            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(user)
            };
        }

        [HttpGet]
        [Route("reset-token-valid/{userId}/{token}")]
        public ActionResult<bool> ResetTokenValid(int userId, Guid token)
        {
            if(!_userRepository.UserPresent(userId))
            {
                return false;
            }

            var user = _userRepository.GetUser(userId);
            return _userHelper.ValidResetToken(user, token);
        }
    }
}
