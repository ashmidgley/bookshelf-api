using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api
{
    [Authorize]
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
        public IEnumerable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        // GET api/categories/1
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Category> Get(int id)
        {
            return _categoryRepository.Get(id);
        }

        // POST api/categories
        [HttpPost]
        public ActionResult<Category> Post([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            int id = _categoryRepository.Add(category);
            return _categoryRepository.Get(id);
        }

        // PUT api/categories
        [HttpPut]
        public ActionResult<Category> Put([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            _categoryRepository.Update(category);
            return _categoryRepository.Get(category.Id);
        }

        // DELETE api/categories
        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.Get(id);
            if (category.Id == default)
            {
                return BadRequest($"Category with id {id} not found.");
            }
            _categoryRepository.Delete(category.Id);
            return category;
        }
    }
}