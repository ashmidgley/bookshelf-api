using NUnit.Framework;
using Bookshelf.Core;
using System;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class EmailHelperShould
    {
        [Test]
        public void ThrowException_OnCallToSendResetToken()
        {
            var email = "testing@bookshelf.com";
            var url = "http://localhost:3000";

            var emailHelper = new EmailHelper();

            Assert.Throws<NotImplementedException>(() => emailHelper.SendResetToken(email, url));
        }
    }
}