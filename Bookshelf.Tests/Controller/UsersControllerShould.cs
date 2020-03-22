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
        UserDtoValidator _userDtoValidator => new UserDtoValidator();

        [Test]
        public void ReturnsAllUsers_WhenCallerAdmin()
        {
            var result = new List<UserDto>();

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.IsAdmin(A<HttpContext>.Ignored)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetAll()).Returns(result);

            var usersController = new UsersController(userRepository, userHelper, _userDtoValidator);

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

            var usersController = new UsersController(userRepository, userHelper, _userDtoValidator);

            var response = usersController.Get(1);

            Assert.AreEqual(result, response.Value);
        }

        [Test]
        public void UpdateUser()
        {
            var updatedUser = new UserDto
            {
                Id = 1,
                Email = "test",
                IsAdmin = true
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.IsAdmin(A<HttpContext>.Ignored)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(A<int>.Ignored)).Returns(true);
            A.CallTo(() => userRepository.GetUser(A<int>.Ignored)).Returns(updatedUser);

            var userController = new UsersController(userRepository, userHelper, _userDtoValidator);

            var responseOne = userController.Update(updatedUser);
            var responseTwo = userController.Update(new UserDto());

            Assert.AreEqual(updatedUser, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateEmail()
        {
            var updatedUser = new UserUpdateDto
            {
                Id = 1,
                Email = "test"
            };

            var user = new UserDto
            {
                Id = 1,
                Email = "test"
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedUser.Id)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(updatedUser.Email)).Returns(false);
            A.CallTo(() => userRepository.GetUser(updatedUser.Id)).Returns(user);

            var userController = new UsersController(userRepository, userHelper, _userDtoValidator);

            var responseOne = userController.UpdateEmail(updatedUser);

            Assert.AreEqual(updatedUser.Email, responseOne.Value.User.Email);
        }

        [Test]
        public void ReturnErrorMessage_WhenEmailExisting()
        {
            var updatedUser = new UserUpdateDto
            {
                Id = 1,
                Email = "existing"
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedUser.Id)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.UserPresent(updatedUser.Email)).Returns(true);

            var userController = new UsersController(userRepository, userHelper, _userDtoValidator);

            var response = userController.UpdateEmail(updatedUser);

            Assert.AreEqual($"Email {updatedUser.Email} is already in use.", response.Value.Error);
        }

        [Test]
        public void UpdatePassword()
        {
            var updatedUser = new UserUpdateDto
            {
                Id = 1,
                Password = "test"
            };

            var user = new UserDto
            {
                Id = 1
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedUser.Id)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetUser(updatedUser.Id)).Returns(user);

            var userController = new UsersController(userRepository, userHelper, _userDtoValidator);

            var response = userController.UpdatePassword(updatedUser);

            A.CallTo(() => userRepository.UpdatePasswordHash(updatedUser.Id, A<string>.Ignored)).MustHaveHappened();
            Assert.AreEqual(updatedUser.Id, response.Value.Id);
        }

        [Test]
        public void DeleteUser()
        {
            var expected = new UserDto
            {
                Id = 1
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.IsAdmin(A<HttpContext>.Ignored)).Returns(false);
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, 1)).Returns(true);

            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetUser(1)).Returns(expected);

            var userController = new UsersController(userRepository, userHelper, _userDtoValidator);

            var responseOne = userController.Delete(1);
            A.CallTo(() => userHelper.DeleteUser(1)).MustHaveHappened();
            Assert.AreEqual(expected.Id, responseOne.Value.Id);

            var responseTwo = userController.Delete(2);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }
    }
}