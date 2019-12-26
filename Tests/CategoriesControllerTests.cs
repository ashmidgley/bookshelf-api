using NUnit.Framework;
using Api;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FakeItEasy;

namespace Tests
{
    public class CategoriesControllerTests
    {
        private CategoryValidator Validator => new CategoryValidator();
        private readonly List<Category> TestCategories = new List<Category>
        {
            new Category 
            {
                Description = "Fiction",
                Code = "🧟"
            },
            new Category 
            { 
                Description = "Non-fiction",
                Code = "🧠"
            }
        };
        private readonly Category CategorySuccess = new Category
        {
            Description = "Sci-fi",
            Code = "🚀"
        };
        private readonly Category CategoryFail = new Category();

        [Test]
        public void GetAllTest()
        {
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.GetAll()).Returns(TestCategories);
            var controller = new CategoriesController(repository, Validator);

            var categories = controller.Get();
            
            Assert.AreEqual(TestCategories, categories);
        }

        [Test]
        public void PostTest()
        {
            const int id = 1;
            var result = CategorySuccess;
            result.Id = id;
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Add(CategorySuccess)).Returns(id);
            A.CallTo(() => repository.Get(id)).Returns(result);
            var controller = new CategoriesController(repository, Validator);

            var responseOne = controller.Post(CategorySuccess);
            var responseTwo = controller.Post(CategoryFail);

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateTest()
        {
            const int id = 1;
            var updatedCategory = CategorySuccess;
            updatedCategory.Id = id;
            updatedCategory.Description = "Updated description...";
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Get(id)).Returns(updatedCategory);
            var controller = new CategoriesController(repository, Validator);

            var responseOne = controller.Put(updatedCategory);
            var responseTwo = controller.Put(CategoryFail);

            Assert.AreEqual(updatedCategory, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteTest()
        {
            const int idSuccess = 1;
            var result = CategorySuccess;
            result.Id = idSuccess;
            const int idFail = 5;
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Get(idSuccess)).Returns(result);
            var controller = new CategoriesController(repository, Validator);
            
            var responseOne = controller.Delete(idSuccess);
            var responseTwo = controller.Delete(idFail);

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            Assert.AreEqual($"Category with id {idFail} not found.", ((BadRequestObjectResult)responseTwo.Result).Value);
        }
    }
}