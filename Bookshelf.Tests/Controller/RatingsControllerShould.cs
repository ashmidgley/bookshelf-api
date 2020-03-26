using NUnit.Framework;
using Bookshelf.Core;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class RatingsControllerShould
    {
        [Test]
        public void ReturnGetRating_OnCallToGetRating()
        {
            var ratingRepository = A.Fake<IRatingRepository>();
            var controller = new RatingsController(ratingRepository, null, null);

            var response = controller.GetRating(1);
            
            A.CallTo(() => ratingRepository.GetRating(1)).MustHaveHappened();
        }

        [Test]
        public void ReturnGetUserRatings_OnCallToGetUserRatings()
        {
            var ratingRepository = A.Fake<IRatingRepository>();
            var controller = new RatingsController(ratingRepository, null, null);

            var ratings = controller.GetUserRatings(1);
            
            A.CallTo(() => ratingRepository.GetUserRatings(1)).MustHaveHappened();
        }

         [Test]
        public void ReturnRating_WhenValidUser_CallsAddRating()
        {
            var newRating = new Rating
            {
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var result = new Rating
            {
                UserId = newRating.UserId
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, newRating.UserId)).Returns(true);

            var ratingRepository = A.Fake<IRatingRepository>();
            A.CallTo(() => ratingRepository.GetRating(A<int>.Ignored)).Returns(result);

            var validator = new RatingValidator();

            var controller = new RatingsController(ratingRepository, userHelper, validator);

            var response = controller.AddRating(newRating);

            A.CallTo(() => ratingRepository.Add(newRating)).MustHaveHappened();
            Assert.AreEqual(result.UserId, response.Value.UserId);
        }

        [Test]
        public void ReturnUnauthorized_WhenInvalidUser_CallsAddRating()
        {
            var newRating = new Rating
            {
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, newRating.UserId)).Returns(false);

            var validator = new RatingValidator();

            var controller = new RatingsController(null, userHelper, validator);

            var response = controller.AddRating(newRating);

            Assert.AreEqual((int)HttpStatusCode.Unauthorized, ((UnauthorizedResult)response.Result).StatusCode);
        }

        [Test]
        public void ReturnRating_WhenValidUser_CallsUpdateRating()
        {
            var updatedRating = new Rating
            {
                Id = 1,
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var result = new Rating
            {
                Id = updatedRating.Id
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedRating.UserId)).Returns(true);

            var ratingRepository = A.Fake<IRatingRepository>();
            A.CallTo(() => ratingRepository.RatingExists(updatedRating.Id)).Returns(true);
            A.CallTo(() => ratingRepository.GetRating(updatedRating.Id)).Returns(result);
           
            var validator = new RatingValidator();

            var controller = new RatingsController(ratingRepository, userHelper, validator);

            var response = controller.UpdateRating(updatedRating);

            A.CallTo(() => ratingRepository.Update(updatedRating)).MustHaveHappened();
            Assert.AreEqual(result.Id, response.Value.Id);
        }

        [Test]
        public void ReturnUnauthorized_WhenInvalidUser_CallsUpdateRating()
        {
            var updatedRating = new Rating
            {
                Id = 1,
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedRating.UserId)).Returns(false);

            var validator = new RatingValidator();

            var controller = new RatingsController(null, userHelper, validator);

            var response = controller.UpdateRating(updatedRating);

            Assert.AreEqual((int)HttpStatusCode.Unauthorized, ((UnauthorizedResult)response.Result).StatusCode);
        }

        [Test]
        public void ReturnBadRequest_WhenRatingDoesNotExist_OnCallToUpdateRating()
        {
            var updatedRating = new Rating
            {
                Id = 1,
                UserId = 1,
                Description = "Test",
                Code = "Test"
            };

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, updatedRating.UserId)).Returns(true);
           
            var ratingRepository = A.Fake<IRatingRepository>();
            A.CallTo(() => ratingRepository.RatingExists(updatedRating.Id)).Returns(false);
           
            var validator = new RatingValidator();

            var controller = new RatingsController(ratingRepository, userHelper, validator);

            var response = controller.UpdateRating(updatedRating);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual($"Rating with Id {updatedRating.Id} does not exist.", ((BadRequestObjectResult)response.Result).Value);
        }

        [Test]
        public void ReturnRating_WhenValidUser_CallsDeleteRating()
        {
            var id = 1;
            
            var result = new Rating
            {
                Id = id,
                UserId = 1
            };

            var ratingRepository = A.Fake<IRatingRepository>();
            A.CallTo(() => ratingRepository.RatingExists(id)).Returns(true);
            A.CallTo(() => ratingRepository.GetRating(id)).Returns(result);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, result.UserId)).Returns(true);

            var controller = new RatingsController(ratingRepository, userHelper, null);

            var response = controller.DeleteRating(id);
            
            A.CallTo(() => ratingRepository.Delete(id)).MustHaveHappened();
            Assert.AreEqual(result.Id, response.Value.Id);
        }

        [Test]
        public void ReturnBadRequest_WhenRatingDoesNotExist_OnCallToDeleteRating()
        {
            var id = 1;

            var ratingRepository = A.Fake<IRatingRepository>();
            A.CallTo(() => ratingRepository.RatingExists(id)).Returns(false);

            var controller = new RatingsController(ratingRepository, null, null);

            var response = controller.DeleteRating(id);
            
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
            Assert.AreEqual($"Rating with Id {id} does not exist.", ((BadRequestObjectResult)response.Result).Value);
        }

        [Test]
        public void ReturnUnauthorized_WhenInvalidUser_CallsDeleteRating()
        {
            var id = 1;
            
            var result = new Rating
            {
                Id = id,
                UserId = 1
            };

            var ratingRepository = A.Fake<IRatingRepository>();
            A.CallTo(() => ratingRepository.RatingExists(id)).Returns(true);
            A.CallTo(() => ratingRepository.GetRating(id)).Returns(result);

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, result.UserId)).Returns(false);

            var controller = new RatingsController(ratingRepository, userHelper, null);

            var response = controller.DeleteRating(id);
            
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, ((UnauthorizedResult)response.Result).StatusCode);
        }
    }
}