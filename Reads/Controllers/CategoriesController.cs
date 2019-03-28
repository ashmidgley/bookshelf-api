using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reads.Models;
using Reads.Validators;

namespace Reads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryValidator _validator;

        public CategoriesController(ICategoryRepository categoryRepository, CategoryValidator validator)
        {
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        // GET api/categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            List<Category> categories = _categoryRepository.GetAll().Result;
            return categories;
        }

        // POST api/categories
        [HttpPost]
        public ActionResult Post([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _categoryRepository.Add(category);
            return Ok();
        }

        // PUT api/categories
        [HttpPut]
        public ActionResult Put([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _categoryRepository.Update(category);
            return Ok();
        }

        // DELETE api/categories
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.Get(id).Result;
            if (category == null)
            {
                return BadRequest($"No category found for id: {id}");
            }
            _categoryRepository.Delete(category);
            return Ok();
        }
    }
}