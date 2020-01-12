using Api;
using FakeItEasy;
using NUnit.Framework;

namespace Tests
{
    public class UsersControllerShould
    {
        LoginDtoValidator _loginValidator => new LoginDtoValidator();
        LoginDto model = new LoginDto
        {
            Email = "Mike Tyson",
            Password = "password"
        };
        UserDto userSuccess = new UserDto
        {
            Id = 1,
            Email = "Mike Tyson"
        };

        [Test]
        public void ReturnTokenAndUser_WhenAuthorizedUserCallsLogin()
        {
            var userHelper = A.Fake<IUserHelper>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.Authenticate(A<LoginDto>.Ignored)).Returns(true);
            A.CallTo(() => userHelper.BuildToken(A<UserDto>.Ignored)).Returns("token");
            A.CallTo(() => userRepository.GetUser(A<string>.Ignored)).Returns(userSuccess);
            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var response = usersController.Login(model);

            Assert.NotNull(response.Value.Token);
            Assert.Null(response.Value.Error);
        }

        [Test]
        public void ReturnError_WhenUnauthorizedUserCallsLogin()
        {
            var userHelper = A.Fake<IUserHelper>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.Authenticate(A<LoginDto>.Ignored)).Returns(false);
            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var response = usersController.Login(model);

            Assert.Null(response.Value.Token);
            Assert.AreEqual("Incorrect credentials. Please try again.", response.Value.Error);
        }

        [Test]
        public void ReturnTokenAndUser_WhenRegisterModelCorrect()
        {
            var userHelper = A.Fake<IUserHelper>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<string>.Ignored)).Returns(false);
            A.CallTo(() => userHelper.BuildToken(A<UserDto>.Ignored)).Returns("token");
            A.CallTo(() => userRepository.GetUser(A<string>.Ignored)).Returns(userSuccess);
            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var response = usersController.Register(model);

            Assert.NotNull(response.Value.Token);
            Assert.Null(response.Value.Error);
        }

        [Test]
        public void ReturnError_WhenRegisteringUsernameAlreadyExists()
        {
            var userHelper = A.Fake<IUserHelper>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<string>.Ignored)).Returns(true);
            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var response = usersController.Register(model);

            Assert.Null(response.Value.Token);
            Assert.AreEqual("Email already in use. Please try another.", response.Value.Error);
        }
    }
}