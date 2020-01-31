using System.Security.Claims;

namespace Bookshelf.Core
{
    public interface IUserHelper
    {
        string BuildToken(UserDto user);
        string HashPassword(string password, byte[] salt = null);
        bool PasswordsMatch(string password, string passwordHash, byte[] salt = null);
        UserDto ToUserDto(User user);
        void Register(int userId);
        bool IsAdmin(ClaimsPrincipal currentUser);
        bool MatchingUsers(ClaimsPrincipal currentUser, int userId);
    }
}