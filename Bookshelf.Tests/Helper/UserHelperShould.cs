using NUnit.Framework;
using Bookshelf.Core;
using FakeItEasy;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class UserHelperShould
    {
        [TestCase("password", "1vEjmwXiHmO98JoEdYcDaQ==", ExpectedResult = "1vEjmwXiHmO98JoEdYcDaY9cJUmKiqXkRihgnJ88NZO7QlTK")]
        public string ReturnHashedPassword(string input, string saltString)
        {
            var salt = Convert.FromBase64String(saltString);
            var userHelper = new UserHelper(null, null, null, null, null);

            return userHelper.HashPassword(input, salt);
        }

        [TestCase("password", "1vEjmwXiHmO98JoEdYcDaY9cJUmKiqXkRihgnJ88NZO7QlTK", "1vEjmwXiHmO98JoEdYcDaQ==", ExpectedResult = true)]
        [TestCase("password123", "1vEjmwXiHmO98JoEdYcDaY9cJUmKiqXkRihgnJ88NZO7QlTK", "1vEjmwXiHmO98JoEdYcDaQ==", ExpectedResult = false)]
        public bool CheckPasswordsMatch(string password, string passwordHash, string saltString)
        {
            var salt = Convert.FromBase64String(saltString);
            var userHelper = new UserHelper(null, null, null, null, null);

            return userHelper.PasswordsMatch(password, passwordHash, salt);
        }

        [Test]
        public void RegisterUserWithDefaultValues()
        {
            var categoryRepository = A.Fake<ICategoryRepository>();
            var ratingRepository = A.Fake<IRatingRepository>();

            var userHelper = new UserHelper(null, null, null, categoryRepository, ratingRepository);

            userHelper.Register(1);
            
            A.CallTo(() => categoryRepository.Add(A<Category>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => ratingRepository.Add(A<Rating>.Ignored)).MustHaveHappened(3, Times.Exactly);
        }

        [TestCase("true", ExpectedResult = true)]
        [TestCase("false", ExpectedResult = false)]
        public bool CheckIfUserInContextIsAdmin(string isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim("IsAdmin", isAdmin)
            };

            var context = A.Fake<HttpContext>();
            context.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(claims));

            var userHelper = new UserHelper(null, null, null, null, null);

            return userHelper.IsAdmin(context);
        }

        [TestCase("1", ExpectedResult = true)]
        [TestCase("2", ExpectedResult = false)]
        public bool CheckIfUserInContextMatchesId(string id)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", id)
            };

            var context = A.Fake<HttpContext>();
            context.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(claims));

            var userHelper = new UserHelper(null, null, null, null, null);

            return userHelper.MatchingUsers(context, 1);
        }

        [TestCase("411a5eae-f119-47be-8d1e-16e3258ecacb", ExpectedResult = true)]
        [TestCase("fe16fe74-ebad-47d1-848d-1b7991119007", ExpectedResult = false)]
        public bool CheckIfPasswordResetTokenMatches(string input)
        {
            var token = new Guid(input);
            var user = new UserDto
            {
                PasswordResetToken = new Guid("411a5eae-f119-47be-8d1e-16e3258ecacb"),
                PasswordResetExpiry = DateTime.Now.AddDays(1)
            };

            var userHelper = new UserHelper(null, null, null, null, null);

            return userHelper.ValidResetToken(user, token);
        }

        [Test]
        public void ReturnTrue_WhenPasswordResetExpiryValid()
        {
            var token = new Guid("411a5eae-f119-47be-8d1e-16e3258ecacb");
            var user = new UserDto
            {
                PasswordResetToken = token,
                PasswordResetExpiry = DateTime.Now.AddDays(1)
            };

            var userHelper = new UserHelper(null, null, null, null, null);

            Assert.IsTrue(userHelper.ValidResetToken(user, token));
        }

        [Test]
        public void ReturnFalse_WhenPasswordResetExpiryInvalid()
        {
            var token = new Guid("411a5eae-f119-47be-8d1e-16e3258ecacb");
            var user = new UserDto
            {
                PasswordResetToken = token,
                PasswordResetExpiry = DateTime.Now.AddDays(-1)
            };

            var userHelper = new UserHelper(null, null, null, null, null);

            Assert.IsFalse(userHelper.ValidResetToken(user, token));
        }

        [Test]
        public void ReturnFalse_WhenPasswordResetExpiryNull()
        {
            var token = new Guid("411a5eae-f119-47be-8d1e-16e3258ecacb");
            var user = new UserDto
            {
                PasswordResetToken = token,
                PasswordResetExpiry = null
            };

            var userHelper = new UserHelper(null, null, null, null, null);

            Assert.IsFalse(userHelper.ValidResetToken(user, token));
        }
    }
}