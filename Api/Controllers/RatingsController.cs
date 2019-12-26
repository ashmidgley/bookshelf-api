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
        public IEnumerable<Rating> GetAll()
        {
            return _ratingRepository.GetAll();
        }

        // GET api/ratings/1
        [HttpGet]
        public ActionResult<Rating> Get(int id)
        {
            return _ratingRepository.Get(id);
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
            return _ratingRepository.Get(id);
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
            return _ratingRepository.Get(rating.Id);
        }

        // DELETE api/ratings
        [HttpDelete("{id}")]
        public ActionResult<Rating> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rating = _ratingRepository.Get(id);
            if (rating.Id == default)
            {
                return BadRequest($"Rating with id {id} not found.");
            }
            _ratingRepository.Delete(rating.Id);
            return rating;
        }
    }
}