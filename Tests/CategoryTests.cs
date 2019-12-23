using NUnit.Framework;
using Bookshelf;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FakeItEasy;

namespace Tests
{
    public class CategoryTests
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
        private readonly Category TestCategory = new Category
        {
            Description = "Sci-fi",
            Code = "🚀"
        };

        [Test]
        public void GetAllTest()
        {
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.GetAll()).Returns(TestCategories);
            var controller = new CategoriesController(repository, Validator);

            var categories = controller.Get().Value;
            
            Assert.AreEqual(TestCategories, categories);
        }

        [Test]
        public void PostTest()
        {
            const int id = 1;
            var categorySuccess = TestCategory;
            var result = TestCategory;
            result.Id = id;
            var categoryFail = new Category();
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Add(categorySuccess)).Returns(id);
            A.CallTo(() => repository.Get(id)).Returns(result);
            var controller = new CategoriesController(repository, Validator);

            var responseOne = controller.Post(categorySuccess);
            categorySuccess.Id = id;
            var responseTwo = controller.Post(categoryFail);

            Assert.AreEqual(categorySuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateTest()
        {
            const int id = 1;
            var categorySuccess = TestCategory;
            categorySuccess.Id = id;
            categorySuccess.Description = "Updated description...";
            var categoryFail = new Category();
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Get(id)).Returns(categorySuccess);
            var controller = new CategoriesController(repository, Validator);

            var responseOne = controller.Put(categorySuccess);
            var responseTwo = controller.Put(categoryFail);

            Assert.AreEqual(categorySuccess, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteTest()
        {
            const int idSuccess = 1;
            var result = TestCategory;
            result.Id = idSuccess;
            const int idFail = 5;
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Get(idSuccess)).Returns(result);
            A.CallTo(() => repository.Get(idFail)).Returns(new Category());
            var controller = new CategoriesController(repository, Validator);
            
            var responseOne = controller.Delete(idSuccess);
            var responseTwo = controller.Delete(idFail);

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
            Assert.AreEqual($"Category with id {idFail} not found.", ((BadRequestObjectResult)responseTwo.Result).Value);
        }
    }
}