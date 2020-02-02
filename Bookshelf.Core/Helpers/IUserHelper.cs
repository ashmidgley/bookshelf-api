using Microsoft.AspNetCore.Http;

namespace Bookshelf.Core
{
    public interface IUserHelper
    {
        string BuildToken(UserDto user);
        string HashPassword(string password, byte[] salt = null);
        bool PasswordsMatch(string password, string passwordHash, byte[] salt = null);
        UserDto ToUserDto(User user);
        void Register(int userId);
        bool IsAdmin(HttpContext context);
        bool MatchingUsers(HttpContext context, int userId);
    }
}