using System.Collections.Generic;
using System.Net;
using Bookshelf.Core;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class UsersControllerShould
    {
        LoginDtoValidator _loginValidator => new LoginDtoValidator();

        [Test]
        public void ReturnsAllUsers_WhenCallerAdmin()
        {
            var result = new List<UserDto>();

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.IsAdmin(A<HttpContext>.Ignored)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetAll()).Returns(result);

            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var response = usersController.GetAll();

            Assert.AreEqual(result, response.Value);
        }

        [Test]
        public void ReturnUser_WhenCallerAdmin()
        {
            var result = new UserDto();

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.IsAdmin(A<HttpContext>.Ignored)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetUser(A<int>.Ignored)).Returns(result);

            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var response = usersController.Get(1);

            Assert.AreEqual(result, response.Value);
        }

        [Test]
        public void ReturnToken_WhenAuthorizedUserCallsLogin()
        {
            var login = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var result = new TokenDto
            {
                Token = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.Authenticate(A<LoginDto>.Ignored)).Returns(true);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.BuildToken(A<UserDto>.Ignored)).Returns("test");

            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var responseOne = usersController.Login(login);
            var responseTwo = usersController.Login(new LoginDto());

            Assert.AreEqual(result.Token, responseOne.Value.Token);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void ReturnError_WhenUnauthorizedUserCallsLogin()
        {
            var login = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.Authenticate(A<LoginDto>.Ignored)).Returns(false);
            var usersController = new UsersController(userRepository, null, _loginValidator);

            var response = usersController.Login(login);

            Assert.Null(response.Value.Token);
            Assert.AreEqual("Incorrect credentials. Please try again.", response.Value.Error);
        }

        [Test]
        public void ReturnToken_WhenRegisterModelCorrect()
        {
            var register = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var result = new TokenDto
            {
                Token = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<string>.Ignored)).Returns(false);
            
            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.BuildToken(A<UserDto>.Ignored)).Returns("test");

            var usersController = new UsersController(userRepository, userHelper, _loginValidator);

            var responseOne = usersController.Register(register);
            var responseTwo = usersController.Register(new LoginDto());

            Assert.AreEqual(result.Token, responseOne.Value.Token);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void ReturnError_WhenRegisteringUsernameAlreadyExists()
        {
            var register = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<string>.Ignored)).Returns(true);

            var usersController = new UsersController(userRepository, null, _loginValidator);

            var response = usersController.Register(register);

            Assert.Null(response.Value.Token);
            Assert.AreEqual("Email already in use. Please try another.", response.Value.Error);
        }
    }
}