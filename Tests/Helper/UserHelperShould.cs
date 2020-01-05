using NUnit.Framework;
using Api;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using System;

namespace Tests
{
    public class UserHelperShould
    {

        [TestCase("password", "1vEjmwXiHmO98JoEdYcDaQ==", ExpectedResult = "1vEjmwXiHmO98JoEdYcDaY9cJUmKiqXkRihgnJ88NZO7QlTK")]
        public string ReturnHashedPassword(string input, string saltString)
        {
            var salt = Convert.FromBase64String(saltString);
            var configuration = A.Fake<IConfiguration>();
            var userHelper = new UserHelper(configuration);

            return userHelper.HashPassword(input, salt);
        }

        [TestCase("password", "1vEjmwXiHmO98JoEdYcDaY9cJUmKiqXkRihgnJ88NZO7QlTK", "1vEjmwXiHmO98JoEdYcDaQ==", ExpectedResult = true)]
        [TestCase("password123", "1vEjmwXiHmO98JoEdYcDaY9cJUmKiqXkRihgnJ88NZO7QlTK", "1vEjmwXiHmO98JoEdYcDaQ==", ExpectedResult = false)]
        public bool CheckPasswordsMatch(string password, string passwordHash, string saltString)
        {
            var salt = Convert.FromBase64String(saltString);
            var configuration = A.Fake<IConfiguration>();
            var userHelper = new UserHelper(configuration);

            return userHelper.PasswordsMatch(password, passwordHash, salt);
        }
    }
}