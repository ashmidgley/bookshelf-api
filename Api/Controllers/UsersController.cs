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

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
