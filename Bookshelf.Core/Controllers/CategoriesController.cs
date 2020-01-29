using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Bookshelf.Core
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

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            return _categoryRepository.GetCategory(id);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("user/{userId}")]
        public IEnumerable<Category> GetUserCategories(int userId)
        {
            return _categoryRepository.GetUserCategories(userId);
        }

        [HttpPost]
        public ActionResult<Category> Post([FromBody] Category category)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != category.UserId)
            {
                return Unauthorized();
            }

            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            int id = _categoryRepository.Add(category);
            
            return _categoryRepository.GetCategory(id);
        }

        [HttpPut]
        public ActionResult<Category> Put([FromBody] Category category)
        {
            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != category.UserId)
            {
                return Unauthorized();
            }

            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            var current = _categoryRepository.GetCategory(category.Id);
            if(current.Id == default)
            {
                return BadRequest($"Category with id {current.Id} not found.");
            }

            _categoryRepository.Update(category);
            
            return _categoryRepository.GetCategory(category.Id);
        }

        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            if(category.Id == default)
            {
                return BadRequest($"Category with id {category.Id} not found.");
            }

            var currentUser = HttpContext.User;
            int userId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value);

            if(userId != category.UserId)
            {
                return Unauthorized();
            }

            _categoryRepository.Delete(category.Id);
            
            return category;
        }
    }
}