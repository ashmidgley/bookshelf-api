using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.Core
{
    public class UserRepository : IUserRepository
    {
        private readonly BookshelfContext _context;
        private readonly IUserHelper _userHelper;

        public UserRepository(BookshelfContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _context.Users
                .Select(u => _userHelper.ToUserDto(u))
                .ToList();
        }

        public UserDto GetUser(string email)
        {
            return _context.Users
                .Select(u => _userHelper.ToUserDto(u))
                .Single(u => u.Email.Equals(email));
        }

        public UserDto GetUser(int id)
        {
            return _context.Users
                .Select(u => _userHelper.ToUserDto(u))
                .Single(u => u.Id == id);
        }

        public int Add(LoginDto login)
        {
            var user = new User
            {
                Email = login.Email,
                PasswordHash = _userHelper.HashPassword(login.Password),
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public bool Authenticate(LoginDto login)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.Email.Equals(login.Email));

            if(user == default)
            {
                return false;
            }
            return _userHelper.PasswordsMatch(login.Password, user.PasswordHash);
        }

        public bool UserPresent(int id)
        {
            return _context.Users
                .Any(u => u.Id == id);
        }

        public bool UserPresent(string email)
        {
            return _context.Users
                .Any(u => u.Email.Equals(email));
        }

        public void Update(UserDto user)
        {
            var currentUser = _context.Users
                .Single(x => x.Id == user.Id);

            currentUser.Email = user.Email;
            currentUser.IsAdmin = user.IsAdmin;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users
                .Single(x => x.Id == id);

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}