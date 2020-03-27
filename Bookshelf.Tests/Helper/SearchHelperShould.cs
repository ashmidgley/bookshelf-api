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
        public void ReturnTheMartian_WhenValidSearchParams_OnCallToSearchGoogleBooks()
        {
            var theMartian = new NewBookDto
            {
                Title = "The Martian",
                Author = "Andy Weir"
            };
            
            var defaultImage = "default.png";
            var config = A.Fake<IConfiguration>();
            A.CallTo(() => config["DefaultCover"]).Returns(defaultImage);
            A.CallTo(() => config["GoogleBooks:Url"]).Returns("");
            A.CallTo(() => config["GoogleBooks:Key"]).Returns("");

            var searchHelper = new SearchHelper(config);

            var result = searchHelper.PullGoogleBooksData(theMartian).Result;

            var martianImage = "http://books.google.com/books/content?id=AvTLDwAAQBAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api";
            var martianPageCount = 384;
            var martianSummary = "Robinson Crusoe on Mars A survival story for the 21st century and the international bestseller behind the major film from Ridley Scott starring Matt Damon and Jessica Chastain. I’m stranded on Mars. I have no way to communicate with Earth. I’m in a Habitat designed to last 31 days. If the Oxygenator breaks down, I’ll suffocate. If the Water Reclaimer breaks down, I’ll die of thirst. If the Hab breaches, I’ll just kind of explode. If none of those things happen, I’ll eventually run out of food and starve to death. So yeah. I’m screwed. Andy Weir's second novel Artemis is now available";

            Assert.AreEqual(martianImage, result.ImageUrl);
            Assert.AreEqual(martianPageCount, result.PageCount);
            Assert.AreEqual(martianSummary, result.Summary);
        }

        [Test]
        [Explicit]
        public void ReturnEmptyBook_WhenInvalidSearchParams_OnCallToSearchGoogleBooks()
        {
            var phonyBook = new NewBookDto
            {
                Title = "asdgasgasdg",
                Author = "asddsgsadg"
            };
            
            var defaultImage = "default.png";
            var config = A.Fake<IConfiguration>();
            A.CallTo(() => config["DefaultCover"]).Returns(defaultImage);
            A.CallTo(() => config["GoogleBooks:Url"]).Returns("");
            A.CallTo(() => config["GoogleBooks:Key"]).Returns("");

            var searchHelper = new SearchHelper(config);

            var result = searchHelper.PullGoogleBooksData(phonyBook).Result;

            Assert.AreEqual(defaultImage, result.ImageUrl);
            Assert.AreEqual(0, result.PageCount);
            Assert.AreEqual(default, result.Summary);
        }
    }
}