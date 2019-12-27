using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
  [Route("api/[controller]")]
  public class TokenController : Controller
  {
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserRepository _userRepository;

        public TokenController(ITokenRepository tokenRepository, IUserRepository userRepository, ITokenHelper tokenHelper) 
        {
          _tokenRepository = tokenRepository;
          _userRepository = userRepository;
          _tokenHelper = tokenHelper;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<TokenDto> CreateToken([FromBody]LoginDto login)
        {
            if (!_tokenRepository.Authenticate(login))
            {
              return Unauthorized();
            }
            var user = _userRepository.GetUser(login.Username);
            return new TokenDto 
            { 
                Token = _tokenHelper.BuildToken(user)
            };
        }
    }
}