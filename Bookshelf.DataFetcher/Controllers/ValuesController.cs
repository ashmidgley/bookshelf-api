using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Bookshelf.DataFetcher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IFetchHelper _helper;

        public ValuesController(IFetchHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        [Route("google-books")]
        public ActionResult<string> GetVolume(string title, string author)
        {
            var book = _helper.PullGoogleBooksData(title, author).Result;
            return JsonSerializer.Serialize(book);
        }

        [HttpGet]
        [Route("google-books/isbn-search")]
        public ActionResult<string> GetVolume(string isbn)
        {
            var book = _helper.PullGoogleBooksData(isbn).Result;
            return JsonSerializer.Serialize(book);
        }

        [HttpGet]
        [Route("open-library")]
        public ActionResult<string> GetBook(string isbn)
        {
            var book = _helper.PullOpenLibraryData(isbn).Result;
            return JsonSerializer.Serialize(book);
        }
    }
}
