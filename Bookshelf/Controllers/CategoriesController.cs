using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Bookshelf.Models;
using Bookshelf.Validators;

namespace Bookshelf.Controllers
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
        public ActionResult<Category> Post([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }
            int id = _categoryRepository.Add(category);
            category.Id = id;
            return category;
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
            return category;
        }

        // DELETE api/categories
        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
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
            return category;
        }
    }
}