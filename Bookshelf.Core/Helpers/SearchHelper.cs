using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Bookshelf.Core
{
    public class SearchHelper : ISearchHelper
    {
        private IGoogleBooksConfiguration _config;
        private readonly HttpClient _client = new HttpClient();
        
        public SearchHelper(IGoogleBooksConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Book>> SearchBooks(string title, string author, int maxResults)
        {
            try
            {
                var search = await SearchGoogleBooks(title.ToLower(), author.ToLower(), maxResults);

                var books = new List<Book>();
                foreach(var item in search.Items)
                {
                    var book = CreateBook(item.VolumeInfo);
                    books.Add(book);
                }

                return books;
            }
            catch(NullReferenceException)
            {
                // No books to deserialize from google books search.
                return new List<Book>();
            }
        }

        private async Task<GoogleBookSearchDto> SearchGoogleBooks(string title, string author, int maxResults)
        {
            var apiUrl = _config.Url;
            var apiKey = _config.Key;
            var encodedTitle = HttpUtility.UrlEncode(title);
            var encodedAuthor = HttpUtility.UrlEncode(author);
            var url = $"{apiUrl}/volumes?q=intitle:{encodedTitle}+inauthor:{encodedAuthor}&maxResults={maxResults}&key={apiKey}";
            var json = await _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<GoogleBookSearchDto>(json);
        }

        private Book CreateBook(VolumeInfo volume)
        {
            var result = new Book
            {
                ImageUrl = _config.DefaultCover
            };

            result.Author = volume.Authors[0];
            result.PageCount = volume.PageCount;
            result.Summary = volume.Description;
            result.Title = volume.Title;

            if(volume.ImageLinks.Thumbnail != null)
            {
                result.ImageUrl = volume.ImageLinks.Thumbnail;
            }

            result.ImageUrl = result.ImageUrl.Replace("http", "https");
            
            return result;
        }
    }
}