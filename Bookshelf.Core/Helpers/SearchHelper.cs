using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Bookshelf.Core
{
    public class SearchHelper : ISearchHelper
    {
        private IConfiguration _config;
        private readonly HttpClient _client = new HttpClient();
        private readonly string _queryParams = "orderBy=relevance&maxResults=1";
        
        public SearchHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> BookExists(NewBookDto book)
        {
            var search = await SearchGoogleBooks(book.Title.ToLower(), book.Author.ToLower());
            return search.TotalItems > 0;
        }

        public async Task<Book> PullBook(NewBookDto book)
        {
            var search = await SearchGoogleBooks(book.Title.ToLower(), book.Author.ToLower());

            if(search.TotalItems == 0)
            {
                throw new Exception($"{book.Title} By {book.Author} not found in Google Books search.");
            }

            return CreateBook(book, search);
        }

        private async Task<GoogleBookSearchDto> SearchGoogleBooks(string title, string author)
        {
            var apiUrl = _config["GoogleBooks:Url"];
            var apiKey = _config["GoogleBooks:Key"];
            var encodedTitle = HttpUtility.UrlEncode(title);
            var encodedAuthor = HttpUtility.UrlEncode(author);
            var url = $"{apiUrl}/volumes?q={encodedTitle}+inauthor:{encodedAuthor}&{_queryParams}&key={apiKey}";
            var json = await _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<GoogleBookSearchDto>(json);
        }

        private Book CreateBook(NewBookDto book, GoogleBookSearchDto search)
        {
            var result = new Book
            {
                UserId = book.UserId,
                CategoryId = book.CategoryId,
                RatingId = book.RatingId,
                FinishedOn = book.FinishedOn,
                ImageUrl = _config["DefaultCover"]
            };

            var volume = search.Items.First().VolumeInfo;
            result.Author = volume.Authors[0];
            result.PageCount = volume.PageCount;
            result.Summary = volume.Description;
            result.Title = volume.Title;

            if(volume.Subtitle != null) {
                result.Title += $" {volume.Subtitle}";
            }
            
            if(volume.ImageLinks.Thumbnail != null)
            {
                result.ImageUrl = volume.ImageLinks.Thumbnail;
            }
            else if(volume.ImageLinks.SmallThumbnail != null)
            {
                result.ImageUrl = volume.ImageLinks.SmallThumbnail;
            }
            result.ImageUrl = result.ImageUrl.Replace("http", "https");
            
            return result;
        }
    }
}