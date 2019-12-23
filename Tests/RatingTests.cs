using NUnit.Framework;
using Api;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FakeItEasy;

namespace Tests
{
    public class RatingTests
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
        private readonly Rating TestRating = new Rating
        { 
            Description = "Extra-mild",
            Code = "🔥"
        };

        [Test]
        public void GetAllTest()
        {
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.GetAll()).Returns(TestRatings);
            var controller = new RatingsController(repository, Validator);

            var ratings = controller.Get().Value.ToList();
            
            Assert.AreEqual(TestRatings, ratings);
        }

        [Test]
        public void PostTest()
        {
            const int id = 1;
            var ratingSuccess = TestRating;
            var result = TestRating;
            result.Id = id;
            var ratingFail = new Rating();
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.Add(ratingSuccess)).Returns(id);
            A.CallTo(() => repository.Get(id)).Returns(result);
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Post(ratingSuccess);
            ratingSuccess.Id = id;
            var responseTwo = controller.Post(ratingFail);

            Assert.AreEqual(ratingSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateTest()
        {
            const int id = 1;
            var ratingSuccess = TestRating;
            ratingSuccess.Id = id;
            ratingSuccess.Description = "Updated description...";
            var ratingFail = new Rating();
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.Get(id)).Returns(ratingSuccess);
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Put(ratingSuccess);
            var responseTwo = controller.Put(ratingFail);

            Assert.AreEqual(ratingSuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteTest()
        {
            const int idSuccess = 1;
            var result = TestRating;
            result.Id = idSuccess;
            const int idFail = 5;
            var repository = A.Fake<IRatingRepository>();
            A.CallTo(() => repository.Get(idSuccess)).Returns(result);
            A.CallTo(() => repository.Get(idFail)).Returns(new Rating());
            var controller = new RatingsController(repository, Validator);

            var responseOne = controller.Delete(idSuccess);
            var responseTwo = controller.Delete(idFail);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            Assert.AreEqual($"Rating with id {idFail} not found.", ((BadRequestObjectResult)responseTwo.Result).Value);
        }
    }
}