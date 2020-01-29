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
        public ActionResult<string> Get(string isbn)
        {
            var book = _helper.PullOpenLibraryData(isbn).Result;
            return JsonSerializer.Serialize(book);
        }
    }
}
