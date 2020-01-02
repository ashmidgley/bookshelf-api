using System.Collections.Generic;

namespace Api
{
    public interface IUserRepository
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetUser(string email);
        UserDto GetUser(int id);
    }
}