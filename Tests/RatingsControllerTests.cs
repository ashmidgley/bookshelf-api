using NUnit.Framework;
using Api;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FakeItEasy;

namespace Tests
{
    public class RatingsControllerTests
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
        public void GetAllTest()
        {
            const int userId = 1;
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetUserRatings(userId)).Returns(TestRatings);
            var controller = new RatingsController(repository, Validator);

            var ratings = controller.GetUserRatings(userId);
            
            Assert.AreEqual(TestRatings, ratings);
        }

        [Test]
        public void PostTest()
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
        public void UpdateTest()
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
        public void DeleteTest()
        {
            const int idSuccess = 1;
            var result = RatingSuccess;
            result.Id = idSuccess;
            const int idFail = 5;
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetRating(idSuccess)).Returns(result);
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Delete(idSuccess);
            var responseTwo = controller.Delete(idFail);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            Assert.AreEqual($"Rating with id {idFail} not found.", ((BadRequestObjectResult)responseTwo.Result).Value);
        }
    }
}