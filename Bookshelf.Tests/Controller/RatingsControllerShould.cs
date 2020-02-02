using NUnit.Framework;
using Bookshelf.Core;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class RatingsControllerShould
    {
        private RatingValidator _ratingValidator => new RatingValidator();

        [Test]
        public void GetAllRatings()
        {
            var result = new List<Rating>();

            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetUserRatings(A<int>.Ignored)).Returns(result);

            var controller = new RatingsController(repository, null, _ratingValidator);

            var ratings = controller.GetUserRatings(1);
            
            Assert.AreEqual(result, ratings);
        }

        [Test]
        public void CreateNewRating()
        {
            var newRating = new Rating
            {
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var result = new Rating();

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.Add(A<Rating>.Ignored)).Returns(1);
            A.CallTo(() => repository.GetRating(A<int>.Ignored)).Returns(result);
            
            var controller = new RatingsController(repository, userHelper, _ratingValidator);

            var responseOne = controller.Post(newRating);
            var responseTwo = controller.Post(new Rating());

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateExistingRating()
        {
            const int id = 1;
            var newRating = new Rating
            {
                Id = id,
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var result = new Rating();
            result.Id = id;

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetRating(id)).Returns(result);
            
            var controller = new RatingsController(repository, userHelper, _ratingValidator);

            var responseOne = controller.Put(newRating);
            var responseTwo = controller.Put(new Rating());

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteRating()
        {
            const int id = 1;
            var result = new Rating();
            result.Id = id;

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetRating(id)).Returns(result);
            
            var controller = new RatingsController(repository, userHelper, _ratingValidator);

            var responseOne = controller.Delete(id);
            var responseTwo = controller.Delete(5);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }
    }
}