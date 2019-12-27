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

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .ToList();
        }

        public User GetUser(string email)
        {
            return _context.Users
                .Single(u => u.Email.Equals(email));
        }

        public User GetUser(int id)
        {
            return _context.Users
                .Single(u => u.Id == id);
        }
    }
}