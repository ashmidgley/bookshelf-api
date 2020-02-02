using NUnit.Framework;
using Bookshelf.Core;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class CategoriesControllerShould
    {
        private CategoryValidator _categoryValidator => new CategoryValidator();

        [Test]
        public void ReturnAllCategories()
        {
            var result = new List<Category>();
            
            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.GetUserCategories(A<int>.Ignored)).Returns(result);

            var controller = new CategoriesController(repository, null, _categoryValidator);

            var categories = controller.GetUserCategories(1);
            
            Assert.AreEqual(result, categories);
        }

        [Test]
        public void CreateNewCategory()
        {
            var newCategory = new Category
            {
                UserId = 1,
                Description = "Test",
                Code = "test"
            };

            var result = new Category();
            
            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.Add(A<Category>.Ignored)).Returns(1);
            A.CallTo(() => repository.GetCategory(A<int>.Ignored)).Returns(result);

            var controller = new CategoriesController(repository, userHelper, _categoryValidator);

            var responseOne = controller.Post(newCategory);
            var responseTwo = controller.Post(new Category());

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void UpdateExistingCategory()
        {
            const int id = 1;
            var updatedCategory = new Category
            {
                Id = id,
                UserId = 1,
                Description = "Test",
                Code = "test"
            };

            var result = new Category();
            result.Id = id;

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.GetCategory(id)).Returns(result);
            
            var controller = new CategoriesController(repository, userHelper, _categoryValidator);

            var responseOne = controller.Put(updatedCategory);
            var responseTwo = controller.Put(new Category());

            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }

        [Test]
        public void DeleteCategory()
        {
            const int id = 1;
            var result = new Category();
            result.Id = id;

            var userHelper = A.Fake<IUserHelper>();
            A.CallTo(() => userHelper.MatchingUsers(A<HttpContext>.Ignored, A<int>.Ignored)).Returns(true);

            var repository = A.Fake<ICategoryRepository>();
            A.CallTo(() => repository.GetCategory(id)).Returns(result);
            
            var controller = new CategoriesController(repository, userHelper, _categoryValidator);

            var responseOne = controller.Delete(id);
            var responseTwo = controller.Delete(5);
            
            Assert.AreEqual(result, responseOne.Value);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)responseTwo.Result).StatusCode);
        }
    }
}