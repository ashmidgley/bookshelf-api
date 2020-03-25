using System;
using System.Net;
using Bookshelf.Core;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class AuthControllerShould
    {
        LoginDtoValidator _loginDtoValidator => new LoginDtoValidator();

        [Test]
        public void ReturnToken_WhenAuthorizedUser_CallsLogin()
        {
            var login = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var token = "test";

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(login.Email)).Returns(true);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.PasswordsMatch(login.Password, A<string>.Ignored, null)).Returns(true);
            A.CallTo(() => userHelper.BuildToken(A<UserDto>.Ignored)).Returns(token);

            var usersController = new AuthController(userRepository, userHelper, _loginDtoValidator);

            var response = usersController.Login(login);

            Assert.AreEqual(token, response.Value);
        }

        [Test]
        public void ReturnError_WhenUnauthorizedEmail_OnCallToLogin()
        {
            var login = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(login.Email)).Returns(false);

            var usersController = new AuthController(userRepository, null, _loginDtoValidator);

            var response = usersController.Login(login);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual($"Incorrect email address. Please try again.", ((BadRequestObjectResult)response.Result).Value);
        }

        [Test]
        public void ReturnError_WhenPasswordDoesNotMatch_OnCallToLogin()
        {
            var login = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(login.Email)).Returns(true);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.PasswordsMatch(login.Password, A<string>.Ignored, null)).Returns(false);

            var usersController = new AuthController(userRepository, userHelper, _loginDtoValidator);

            var response = usersController.Login(login);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual($"Incorrect password. Please try again.", ((BadRequestObjectResult)response.Result).Value);
        }

        [Test]
        public void ReturnToken_WhenCorrectRegisterModel_OnCallToRegister()
        {
            var register = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var token = "test";

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<string>.Ignored)).Returns(false);
            
            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.BuildToken(A<UserDto>.Ignored)).Returns(token);

            var usersController = new AuthController(userRepository, userHelper, _loginDtoValidator);

            var responseOne = usersController.Register(register);
            var responseTwo = usersController.Register(new LoginDto());

            Assert.AreEqual(token, responseOne.Value);
        }

        [Test]
        public void ReturnError_WhenUsernameAlreadyExists_OnCallToRegister()
        {
            var register = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<string>.Ignored)).Returns(true);

            var usersController = new AuthController(userRepository, null, _loginDtoValidator);

            var response = usersController.Register(register);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual("Email already in use. Please try another.", ((BadRequestObjectResult)response.Result).Value);
        }

        [Test]
        public void ReturnUserDto_WhenResetTokenValid_OnCallToUpdatePasswordUsingToken()
        {
            var model = new ResetTokenUpdateDto 
            {
                UserId = 1,
                Token = Guid.NewGuid(),
                Password = "test"
            };
            
            var user = new UserDto
            {
                Id = 1
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(model.UserId)).Returns(true);
            A.CallTo(() => userRepository.GetUser(model.UserId)).Returns(user);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.ValidResetToken(user, model.Token)).Returns(true);

            var controller = new AuthController(userRepository, userHelper, null);

            var response = controller.UpdatePasswordUsingToken(model);

            A.CallTo(() => userRepository.UpdatePasswordHash(model.UserId, A<string>.Ignored)).MustHaveHappened();
            A.CallTo(() => userRepository.SetPasswordResetFields(model.UserId, null, null)).MustHaveHappened();
            Assert.AreEqual(model.UserId, response.Value.Id);
        }

        [Test]
        public void ReturnBadRequest_WhenUserNotPresent_OnCallToUpdatePasswordUsingToken()
        {
            var model = new ResetTokenUpdateDto 
            {
                UserId = 1,
                Token = Guid.NewGuid(),
                Password = "test"
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(model.UserId)).Returns(false);

            var controller = new AuthController(userRepository, null, null);

            var response = controller.UpdatePasswordUsingToken(model);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual($"User with Id {model.UserId} does not exist.", ((BadRequestObjectResult)response.Result).Value);
        }

        [Test]
        public void ReturnBadRequest_WhenTokenInvalid_OnCallToUpdatePasswordUsingToken()
        {
            var model = new ResetTokenUpdateDto 
            {
                UserId = 1,
                Token = Guid.NewGuid(),
                Password = "test"
            };

            var user = new UserDto
            {
                Id = 1
            };

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(model.UserId)).Returns(true);
            A.CallTo(() => userRepository.GetUser(model.UserId)).Returns(user);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.ValidResetToken(user, model.Token)).Returns(false);

            var controller = new AuthController(userRepository, userHelper, null);

            var response = controller.UpdatePasswordUsingToken(model);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual("Password reset token is not valid.", ((BadRequestObjectResult)response.Result).Value);
        }
    }
}