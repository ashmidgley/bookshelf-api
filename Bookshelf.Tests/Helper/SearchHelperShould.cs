using NUnit.Framework;
using Bookshelf.Core;
using FakeItEasy;
using System.Threading.Tasks;
using System.Linq;

namespace Bookshelf.Tests
{
    [TestFixture]
    public class SearchHelperShould
    {
        [Test]
        [Explicit]
        public async Task ReturnTheMartian_WhenValidSearchParams_OnCallToSearchBooks()
        {
            var search = new SearchDto
            {
                Title = "The Martian",
                Author = "Andy Weir",
                MaxResults = 1
            };

            var defaultImage = "default.png";
            var config = A.Fake<IGoogleBooksConfiguration>();
            A.CallTo(() => config.DefaultCover).Returns(defaultImage);
            A.CallTo(() => config.Url).Returns("");
            A.CallTo(() => config.Key).Returns("");

            var searchHelper = new SearchHelper(config);

            var result = await searchHelper.SearchBooks(search);
            var book = result.First();

            var martianImage = "https://books.google.com/books/content?id=AvTLDwAAQBAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api";
            var martianPageCount = 384;
            var martianSummary = "Robinson Crusoe on Mars A survival story for the 21st century and the international bestseller behind the major film from Ridley Scott starring Matt Damon and Jessica Chastain. I’m stranded on Mars. I have no way to communicate with Earth. I’m in a Habitat designed to last 31 days. If the Oxygenator breaks down, I’ll suffocate. If the Water Reclaimer breaks down, I’ll die of thirst. If the Hab breaches, I’ll just kind of explode. If none of those things happen, I’ll eventually run out of food and starve to death. So yeah. I’m screwed. Andy Weir's second novel Artemis is now available";

            Assert.AreEqual(martianImage, book.ImageUrl);
            Assert.AreEqual(martianPageCount, book.PageCount);
            Assert.AreEqual(martianSummary, book.Summary);
        }

        [Test]
        [Explicit]
        public async Task ReturnEmptyList_WhenInvalidSearchParams_OnCallToSearchBooks()
        {
            var search = new SearchDto
            {
                Title = "asdgasgasdg",
                Author = "asdgasgasdg",
                MaxResults = 1
            };

            var defaultImage = "default.png";
            var config = A.Fake<IGoogleBooksConfiguration>();
            A.CallTo(() => config.DefaultCover).Returns(defaultImage);
            A.CallTo(() => config.Url).Returns("");
            A.CallTo(() => config.Key).Returns("");

            var searchHelper = new SearchHelper(config);

            var result = await searchHelper.SearchBooks(search);

            Assert.AreEqual(0, result.Count());
        }
    }
}