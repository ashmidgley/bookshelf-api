using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly RatingValidator _validator;

        public RatingsController(IRatingRepository ratingRepository, RatingValidator validator)
        {
            _ratingRepository = ratingRepository;
            _validator = validator;
        }

        // GET api/ratings/1
        [HttpGet]
        [Route("{ratingId}")]
        public ActionResult<Rating> Get(int ratingId)
        {
            return _ratingRepository.GetRating(ratingId);
        }

        // GET api/ratings/user/1
        [HttpGet]
        [Route("user/{userId}")]
        public IEnumerable<Rating> GetUserRatings(int userId)
        {
            return _ratingRepository.GetUserRatings(userId);
        }

        // POST api/ratings
        [HttpPost]
        public ActionResult<Rating> Post([FromBody] Rating rating)
        {
            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            var id = _ratingRepository.Add(rating);
            return _ratingRepository.GetRating(id);
        }

        // PUT api/ratings
        [HttpPut]
        public ActionResult<Rating> Put([FromBody] Rating rating)
        {
            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _ratingRepository.Update(rating);
            return _ratingRepository.GetRating(rating.Id);
        }

        // DELETE api/ratings
        [HttpDelete("{id}")]
        public ActionResult<Rating> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rating = _ratingRepository.GetRating(id);
            if (rating.Id == default)
            {
                return BadRequest($"Rating with id {id} not found.");
            }
            _ratingRepository.Delete(rating.Id);
            return rating;
        }
    }
}