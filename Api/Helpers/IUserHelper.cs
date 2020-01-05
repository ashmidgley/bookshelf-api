namespace Api
{
    public interface IUserHelper
    {
        string BuildToken();
        string HashPassword(string password, byte[] salt = null);
        bool PasswordsMatch(string password, string passwordHash, byte[] salt = null);
    }
}