using NUnit.Framework;
using Bookshelf.Core;
using Microsoft.Extensions.Configuration;
using FakeItEasy;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class SearchHelperShould
    {
        [Test]
        [Explicit]
        public void SearchGoogleBooks()
        {
            var theMartian = new NewBookDto
            {
                Title = "The Martian",
                Author = "Andy Weir"
            };

            var phonyBook = new NewBookDto
            {
                Title = "asdgasgasdg",
                Author = "asddsgsadg"
            };
            
            var defaultImage = "default.png";
            var config = A.Fake<IConfiguration>();
            A.CallTo(() => config["DefaultCover"]).Returns(defaultImage);
            A.CallTo(() => config["GoogleBooks:Url"]).Returns("https://www.googleapis.com/books/v1");
            A.CallTo(() => config["GoogleBooks:Key"]).Returns("");

            var searchHelper = new SearchHelper(config);

            var resultOne = searchHelper.PullGoogleBooksData(theMartian).Result;
            var resultTwo = searchHelper.PullGoogleBooksData(phonyBook).Result;

            var martianImage = "http://books.google.com/books/content?id=AvTLDwAAQBAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api";
            var martianPageCount = 384;
            var martianSummary = "Robinson Crusoe on Mars A survival story for the 21st century and the international bestseller behind the major film from Ridley Scott starring Matt Damon and Jessica Chastain. I’m stranded on Mars. I have no way to communicate with Earth. I’m in a Habitat designed to last 31 days. If the Oxygenator breaks down, I’ll suffocate. If the Water Reclaimer breaks down, I’ll die of thirst. If the Hab breaches, I’ll just kind of explode. If none of those things happen, I’ll eventually run out of food and starve to death. So yeah. I’m screwed. Andy Weir's second novel Artemis is now available";

            Assert.IsTrue(resultOne.ImageUrl == martianImage && resultOne.PageCount == martianPageCount && resultOne.Summary == martianSummary);
            Assert.IsTrue(resultTwo.ImageUrl == defaultImage && resultTwo.PageCount == default && resultTwo.Summary == default);
        }
    }
}