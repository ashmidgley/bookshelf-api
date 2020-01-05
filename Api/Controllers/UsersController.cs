using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
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

        // GET api/users
        [HttpGet]
        public IEnumerable<UserDto> GetAll()
        {
            return _userRepository.GetAll();
        }

        // GET api/users/1
        [HttpGet]
        [Route("{id}")]
        public ActionResult<UserDto> Get(int id)
        {
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
            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(),
                User = _userRepository.GetUser(login.Email)
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
            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(),
                User = _userRepository.GetUser(id)
            };
        }
    }
}
