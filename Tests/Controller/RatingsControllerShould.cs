using NUnit.Framework;
using Api;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FakeItEasy;

namespace Tests
{
    public class RatingsControllerShould
    {
        private RatingValidator Validator => new RatingValidator();
        private readonly List<Rating> TestRatings = new List<Rating>
        {
            new Rating 
            { 
                Description = "Mild",
                Code = "🔥"
            },
            new Rating 
            { 
                Description = "Medium",
                Code = "🔥🔥"
            }
        };
        private readonly Rating RatingSuccess = new Rating
        { 
            Description = "Extra-mild",
            Code = "🔥"
        };
        private readonly Rating RatingFail = new Rating();

        [Test]
        public void GetAllRatings()
        {
            const int userId = 1;
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetUserRatings(userId)).Returns(TestRatings);
            var controller = new RatingsController(repository, Validator);

            var ratings = controller.GetUserRatings(userId);
            
            Assert.AreEqual(TestRatings, ratings);
        }

        [Test]
        public void CreateNewRating()
        {
            const int id = 1;
            var result = RatingSuccess;
            result.Id = id;
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.Add(RatingSuccess)).Returns(id);
            A.CallTo(() => repository.GetRating(id)).Returns(result);
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Post(RatingSuccess);
            var responseTwo = controller.Post(RatingFail);

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateExistingRating()
        {
            const int id = 1;
            var updatedRating = RatingSuccess;
            updatedRating.Id = id;
            updatedRating.Description = "Updated description...";
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetRating(id)).Returns(updatedRating);
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Put(updatedRating);
            var responseTwo = controller.Put(RatingFail);

            Assert.AreEqual(updatedRating, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteRating()
        {
            var result = RatingSuccess;
            result.Id = 1;
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetRating(A<int>.Ignored)).Returns(result);
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Delete(RatingSuccess);
            var responseTwo = controller.Delete(RatingFail);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }
    }
}