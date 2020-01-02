using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class UserRepository : IUserRepository
    {
        private readonly BookshelfContext _context;

        public UserRepository(BookshelfContext context)
        {
            _context = context;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _context.Users
                .Select(u => new UserDto { Id = u.Id, Email = u.Email })
                .ToList();
        }

        public UserDto GetUser(string email)
        {
            return _context.Users
                .Select(u => new UserDto { Id = u.Id, Email = u.Email })
                .Single(u => u.Email.Equals(email));
        }

        public UserDto GetUser(int id)
        {
            return _context.Users
                .Select(u => new UserDto { Id = u.Id, Email = u.Email })
                .Single(u => u.Id == id);
        }
    }
}