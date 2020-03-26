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
        private readonly IUserHelper _userHelper;
        private readonly CategoryValidator _validator;

        public CategoriesController(ICategoryRepository categoryRepository, IUserHelper userHelper, CategoryValidator validator)
        {
            _categoryRepository = categoryRepository;
            _userHelper = userHelper;
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
        public ActionResult<Category> AddCategory([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userHelper.MatchingUsers(HttpContext, category.UserId))
            {
                return Unauthorized();
            }
            
            var id = _categoryRepository.Add(category);
            return _categoryRepository.GetCategory(id);
        }

        [HttpPut]
        public ActionResult<Category> UpdateCategory([FromBody] Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return BadRequest(validation.ToString());
            }

            if(!_userHelper.MatchingUsers(HttpContext, category.UserId))
            {
                return Unauthorized();
            }

            if(!_categoryRepository.CategoryExists(category.Id))
            {
                return BadRequest($"Category with Id {category.Id} does not exist.");
            }

            _categoryRepository.Update(category);
            return _categoryRepository.GetCategory(category.Id);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<Category> DeleteCategory(int id)
        {
            if(!_categoryRepository.CategoryExists(id))
            {
                return BadRequest($"Category with Id {id} does not exist.");
            }

            var category = _categoryRepository.GetCategory(id);
            if(!_userHelper.MatchingUsers(HttpContext, category.UserId))
            {
                return Unauthorized();
            }

            _categoryRepository.Delete(category.Id);
            return category;
        }
    }
}