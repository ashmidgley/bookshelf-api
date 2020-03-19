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
    }
}