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

        // GET api/ratings
        [HttpGet]
        public ActionResult<IEnumerable<Rating>> Get()
        {
            List<Rating> ratings = _ratingRepository.GetAll().Result;
            return ratings;
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
            int id = _ratingRepository.Add(rating);
            rating.Id = id;
            return rating;
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
            return rating;
        }

        // DELETE api/ratings
        [HttpDelete("{id}")]
        public ActionResult<Rating> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rating = _ratingRepository.Get(id).Result;
            if (rating == null)
            {
                return BadRequest($"No rating found for id: {id}");
            }
            _ratingRepository.Delete(rating);
            return rating;
        }
    }
}