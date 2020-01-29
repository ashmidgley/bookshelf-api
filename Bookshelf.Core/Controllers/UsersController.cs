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

        public UsersController(IUserRepository userRepository, IUserHelper userHelper, LoginDtoValidator loginValidator)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
            _loginValidator = loginValidator;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetAll()
        {
            var currentUser = HttpContext.User;
            bool admin = bool.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("IsAdmin")).Value);

            if(!admin)
            {
                return Unauthorized();
            }
            
            return _userRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<UserDto> Get(int id)
        {
            var currentUser = HttpContext.User;
            bool admin = bool.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("IsAdmin")).Value);

            if(!admin)
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
    }
}
