using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http;
using System.Linq;

namespace Bookshelf.DataFetcher
{
    public class FetchHelper : IFetchHelper
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _defaultCover = "https://bulma.io/images/placeholders/256x256.png";
        private readonly string _openLibraryApiUrl = "http://openlibrary.org/api";

        public async Task<BookDto> PullOpenLibraryData(string isbn)
        {
            var key = $"ISBN:{isbn}";
            var book = new BookDto
            {
                ImageUrl = _defaultCover
            };

            var dataDictionary = await GetData(key);
            if(dataDictionary.Count != 0)
            {
                var data = dataDictionary[key];
                book.Title = data.Title;
                book.Author = data.Authors?.First().Name ?? default;
                book.PageCount = data.PageCount;
                book.ImageUrl = data.Covers?.Url ?? _defaultCover;
            }

            var detailsDictionary = await GetDetails(key);
            if(detailsDictionary.Count != 0)
            {
                var details = detailsDictionary[key]; 
                book.Summary = details.Details.Description?.Value ?? default;
            }

            return book;
        }

        private async Task<Dictionary<string, OpenLibraryData>> GetData(string isbn)
        {
            var json = await _client.GetStringAsync($"{_openLibraryApiUrl}/books?bibkeys={isbn}&format=json&jscmd=data");
            return JsonSerializer.Deserialize<Dictionary<string, OpenLibraryData>>(json);
        }

        private async Task<Dictionary<string, OpenLibraryDetails>> GetDetails(string isbn)
        {
            var json = await _client.GetStringAsync($"{_openLibraryApiUrl}/books?bibkeys={isbn}&format=json&jscmd=details");
            return JsonSerializer.Deserialize<Dictionary<string, OpenLibraryDetails>>(json);
        }
    }
}