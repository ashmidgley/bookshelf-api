using Api;
using FakeItEasy;
using NUnit.Framework;

namespace Tests
{
    public class UsersControllerShould
    {
        [Test]
        public void ReturnTokenDto_WhenAuthorizedUserCallsLogin()
        {
            var login = new LoginDto
            {
                Username = "Mike Tyson",
                Password = "password"
            };
            var user = new UserDto
            {
                Id = 1,
                Email = "Mike Tyson"
            };
            var userHelper = A.Fake<IUserHelper>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userHelper.BuildToken()).Returns("token");
            A.CallTo(() => userRepository.Authenticate(A<LoginDto>.Ignored)).Returns(true);
            A.CallTo(() => userRepository.GetUser(A<string>.Ignored)).Returns(user);
            var usersController = new UsersController(userRepository, userHelper);

            var response = usersController.Login(login);

            Assert.IsInstanceOf<TokenDto>(response.Value);
        }
    }
}