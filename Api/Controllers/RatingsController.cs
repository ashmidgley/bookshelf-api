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
            return _ratingRepository.GetAll().Result;
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
            var id = _ratingRepository.Add(rating).Result;
            return _ratingRepository.Get(id).Result;
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
            return _ratingRepository.Get(rating.Id).Result;
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
            if (rating.Id == default)
            {
                return BadRequest($"Rating with id {id} not found.");
            }
            _ratingRepository.Delete(rating);
            return rating;
        }
    }
}