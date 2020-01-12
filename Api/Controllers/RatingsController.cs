using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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

        [HttpGet]
        [Route("{ratingId}")]
        public ActionResult<Rating> Get(int ratingId)
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
        public ActionResult<Rating> Post([FromBody] Rating rating)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);
            
            if(userId != rating.UserId)
            {
                return Unauthorized();
            }

            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var id = _ratingRepository.Add(rating);

            return _ratingRepository.GetRating(id);
        }

        [HttpPut]
        public ActionResult<Rating> Put([FromBody] Rating rating)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != rating.UserId)
            {
                return Unauthorized();
            }

            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var current = _ratingRepository.GetRating(rating.Id);
            if(current.Id == default)
            {
                return BadRequest($"Rating with id {current.Id} not found.");
            }

            _ratingRepository.Update(rating);

            return _ratingRepository.GetRating(rating.Id);
        }

        [HttpDelete]
        public ActionResult<Rating> Delete([FromBody] Rating rating)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != rating.UserId)
            {
                return Unauthorized();
            }

            var validation = _validator.Validate(rating);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var current = _ratingRepository.GetRating(rating.Id);
            if(current.Id == default)
            {
                return BadRequest($"Rating with id {current.Id} not found.");
            }

            _ratingRepository.Delete(rating.Id);

            return current;
        }
    }
}