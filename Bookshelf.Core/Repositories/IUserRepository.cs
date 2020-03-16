using System.Collections.Generic;

namespace Bookshelf.Core
{
    public interface IUserRepository
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetUser(string email);
        UserDto GetUser(int id);
        int Add(LoginDto login);
        void Update(UserDto user);
        void UpdatePasswordHash(int id, string passwordHash);
        void Delete(int id);
        bool UserPresent(int id);
        bool UserPresent(string email);
        bool Authenticate(LoginDto login);
    }
}