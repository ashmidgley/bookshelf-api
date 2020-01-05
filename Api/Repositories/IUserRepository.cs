using System.Collections.Generic;

namespace Api
{
    public interface IUserRepository
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetUser(string email);
        UserDto GetUser(int id);
        int Add(LoginDto login);
        bool UserPresent(string email);
        bool Authenticate(LoginDto login);
    }
}