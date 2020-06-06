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
        private readonly IGoogleBooksConfiguration _config;
        private readonly HttpClient _client = new HttpClient();
        
        public SearchHelper(IGoogleBooksConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Book>> SearchBooks(SearchDto search)
        {
            var encodedTitle = HttpUtility.UrlEncode(search.Title.ToLower());
            var encodedAuthor = HttpUtility.UrlEncode(search.Author.ToLower());
            var url = $"{_config.Url}/volumes?q=intitle:{encodedTitle}+inauthor:{encodedAuthor}&maxResults={search.MaxResults}&key={_config.Key}";

            try
            {
                var results = await Search(url);

                var books = new List<Book>();
                foreach(var item in results.Items)
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

        public async Task<IEnumerable<Book>> SearchBooksByTitle(SearchTitleDto search)
        {
            var encodedTitle = HttpUtility.UrlEncode(search.Title.ToLower());
            var url = $"{_config.Url}/volumes?q=intitle:{encodedTitle}&maxResults={search.MaxResults}&key={_config.Key}";

            try
            {
                var results = await Search(url);

                var books = new List<Book>();
                foreach(var item in results.Items)
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

        public async Task<IEnumerable<Book>> SearchBooksByAuthor(SearchAuthorDto search)
        {
            var encodedAuthor = HttpUtility.UrlEncode(search.Author.ToLower());
            var url = $"{_config.Url}/volumes?q=inauthor:{encodedAuthor}&maxResults={search.MaxResults}&key={_config.Key}";

            try
            {
                var results = await Search(url);

                var books = new List<Book>();
                foreach(var item in results.Items)
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

        private async Task<GoogleBookSearchDto> Search(string url)
        {
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