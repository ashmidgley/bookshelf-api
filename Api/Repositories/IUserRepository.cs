using System.Collections.Generic;

namespace Api
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetUser(string email);
        User GetUser(int id);
    }
}