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

        public UsersController(IUserRepository userRepository, IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
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
            if (!_userRepository.Authenticate(login))
            {
                return Unauthorized();
            }
            return new TokenDto 
            { 
                Token = _userHelper.BuildToken(),
                User = _userRepository.GetUser(login.Username)
            };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult<TokenDto> Register(LoginDto login)
        {
            if(_userRepository.UserPresent(login.Username)) 
            {
                return Unauthorized();
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
