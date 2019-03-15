using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reads;
using Reads.Models;

namespace Reads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET api/categories
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<Category> categories = _categoryRepository.GetAll().Result;
            return Ok(categories);
        }

        // POST api/categories
        [HttpPost]
        public ActionResult Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _categoryRepository.Add(category);
            return Ok();
        }

        // PUT api/categories
        [HttpPut]
        public ActionResult Put([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
            _categoryRepository.Delete(category);
            return Ok();
        }
    }
}