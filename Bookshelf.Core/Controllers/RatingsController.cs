using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Bookshelf.Core
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUserHelper _userHelper;
        private readonly RatingValidator _validator;

        public RatingsController(IRatingRepository ratingRepository, IUserHelper userHelper, RatingValidator validator)
        {
            _ratingRepository = ratingRepository;
            _userHelper = userHelper;
            _validator = validator;
        }

        [HttpGet]
        [Route("{ratingId}")]
        public ActionResult<Rating> GetRating(int ratingId)
        {
            return _ratingRepository.GetRating(ratingId);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("user/{userId}")]
        public IEnumerable<Rating> GetUserRatings(int userId)
        {
            return _ratingRepository.GetUserRatings(userId);
        }

        [HttpPost]
        public ActionResult<Rating> AddRating([FromBody] Rating rating)
        {
            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userHelper.MatchingUsers(HttpContext, rating.UserId))
            {
                return Unauthorized();
            }

            var id = _ratingRepository.Add(rating);
            return _ratingRepository.GetRating(id);
        }

        [HttpPut]
        public ActionResult<Rating> UpdateRating([FromBody] Rating rating)
        {
            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userHelper.MatchingUsers(HttpContext, rating.UserId))
            {
                return Unauthorized();
            }

            if(!_ratingRepository.RatingExists(rating.Id))
            {
                return BadRequest($"Rating with Id {rating.Id} does not exist.");
            }

            _ratingRepository.Update(rating);
            return _ratingRepository.GetRating(rating.Id);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<Rating> DeleteRating(int id)
        {
            if(!_ratingRepository.RatingExists(id))
            {
                return BadRequest($"Rating with Id {id} does not exist.");
            }
            
            var rating = _ratingRepository.GetRating(id);
            if(!_userHelper.MatchingUsers(HttpContext, rating.UserId))
            {
                return Unauthorized();
            }

            _ratingRepository.Delete(id);
            return rating;
        }
    }
}