using System.Linq;

namespace Api
{
    public class TokenRepository : ITokenRepository
    {
        private BookshelfContext _context;

        public TokenRepository(BookshelfContext context)
        {
            _context = context;
        }

        public bool Authenticate(LoginDto login)
        {
            return _context.Users
                .Any(u => u.Email.Equals(login.Username) && u.Password.Equals(login.Password));
        }
    }
}