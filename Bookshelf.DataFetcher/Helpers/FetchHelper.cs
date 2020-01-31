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
        private readonly string _googleBooksApiUrl = "https://www.googleapis.com/books/v1";
        private readonly string _apiKey = "AIzaSyCXxqGUxHHIKTglmzBQKA9sLTVSV8Q6yJw";
        private readonly string _queryParams = "printType=books&orderBy=relevance&maxResults=1";

        public async Task<GoogleBooksDto> PullGoogleBooksData(string title, string author)
        {
            var result = new GoogleBooksDto
            {
                ImageUrl = _defaultCover
            };

            var search = await SearchGoogleBooks(title.ToLower(), author.ToLower());

            if(search.TotalItems > 0)
            {
                var volume = search.Items.First().VolumeInfo;
                result.PageCount = volume.PageCount;
                
                if(volume.ImageLinks.Small != null)
                {
                    result.ImageUrl = volume.ImageLinks.Small;
                }
                else if(volume.ImageLinks.Thumbnail != null)
                {
                    result.ImageUrl = volume.ImageLinks.Thumbnail;
                }

                result.Summary = volume.Description;
            }

            return result;
        }

        public async Task<IsbnDto> PullGoogleBooksData(string isbn)
        {
            var result = new IsbnDto
            {
                ImageUrl = _defaultCover
            };

            var search = await SearchGoogleBooks(isbn);

            if(search.TotalItems > 0)
            {
                var volume = search.Items.First().VolumeInfo;
                result.Title = volume.Title;
                
                if(volume.Subtitle != null)
                {
                    result.Title += $" - {volume.Subtitle}";
                }

                result.Author = volume.Authors[0];
                result.PageCount = volume.PageCount;

                if(volume.ImageLinks.Small != null)
                {
                    result.ImageUrl = volume.ImageLinks.Small;
                }
                else if(volume.ImageLinks.Thumbnail != null)
                {
                    result.ImageUrl = volume.ImageLinks.Thumbnail;
                }

                result.Summary = volume.Description;
            }

            return result;
        }

        private async Task<GoogleBookSearch> SearchGoogleBooks(string title, string author)
        {
            var url = $"{_googleBooksApiUrl}/volumes?q={title}+inauthor:{author}&{_queryParams}&key={_apiKey}";
            var json = await _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<GoogleBookSearch>(json);
        }

        private async Task<GoogleBookSearch> SearchGoogleBooks(string isbn)
        {
            var url = $"{_googleBooksApiUrl}/volumes?q={isbn}&{_queryParams}&key={_apiKey}";
            var json = await _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<GoogleBookSearch>(json);
        }

        public async Task<IsbnDto> PullOpenLibraryData(string isbn)
        {
            var key = $"ISBN:{isbn}";
            var book = new IsbnDto
            {
                ImageUrl = _defaultCover
            };

            var dataDictionary = await GetOpenLibraryData(key);
            if(dataDictionary.Count != 0)
            {
                var data = dataDictionary[key];
                book.Title = data.Title;
                book.Author = data.Authors?.First().Name ?? null;
                book.PageCount = data.PageCount;
                book.ImageUrl = data.Covers?.Url ?? _defaultCover;
            }

            var detailsDictionary = await GetOpenLibraryDetails(key);
            if(detailsDictionary.Count != 0)
            {
                var details = detailsDictionary[key]; 
                book.Summary = details.Details.Description?.Value ?? null;
            }

            return book;
        }

        private async Task<Dictionary<string, OpenLibraryData>> GetOpenLibraryData(string isbn)
        {
            var url = $"{_openLibraryApiUrl}/books?bibkeys={isbn}&format=json&jscmd=data";
            var json = await _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<Dictionary<string, OpenLibraryData>>(json);
        }

        private async Task<Dictionary<string, OpenLibraryDetails>> GetOpenLibraryDetails(string isbn)
        {
            var url = $"{_openLibraryApiUrl}/books?bibkeys={isbn}&format=json&jscmd=details";
            var json = await _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<Dictionary<string, OpenLibraryDetails>>(json);
        }
    }
}