using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api
{
    public class BookHelper : IBookHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string defaultCover = "https://bulma.io/images/placeholders/256x256.png";
        
        public BookDto ToBookDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                UserId = book.UserId,
                CategoryId = book.CategoryId,
                RatingId = book.RatingId,
                ImageUrl = book.ImageUrl,
                Title = book.Title,
                Author = book.Author,
                FinishedOn = book.FinishedOn,
                Year = book.FinishedOn.Year,
                PageCount = book.PageCount,
                Summary = book.Summary
            };
        }

        public Book ToBook(BookDto dto)
        {
            return new Book
            {
                Id = dto.Id,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                RatingId = dto.RatingId,
                ImageUrl = dto.ImageUrl,
                Title = dto.Title,
                Author = dto.Author,
                FinishedOn = dto.FinishedOn,
                PageCount = dto.PageCount,
                Summary = dto.Summary
            };
        }

        public async Task<Book> PullOpenLibraryData(NewBookDto newBook)
        {
            var isbn = $"ISBN:{newBook.ISBN}";
            var book = new Book
            {
                UserId = newBook.UserId,
                CategoryId = newBook.CategoryId,
                RatingId = newBook.RatingId,
                FinishedOn = newBook.FinishedOn,
                ImageUrl = defaultCover
            };

            var dataDictionary = await GetData(isbn);
            if(dataDictionary.Count == 0)
            {
                return book;
            }

            var data = dataDictionary[isbn];
            book.ImageUrl = data.Covers?.Url ?? defaultCover;
            book.Title = data.Title;
            book.Author = data.Authors?.First().Name ?? default;
            book.PageCount = data.PageCount;

            var detailsDictionary = await GetDetails(isbn);
            if(detailsDictionary.Count == 0)
            {
                return book;
            }
            
            var details = detailsDictionary[isbn]; 
            book.Summary = details.Details.Description?.Value ?? default;

            return book;
        }

         private static async Task<Dictionary<string, OpenLibraryData>> GetData(string isbn)
        {
            var json = await client.GetStringAsync($"http://openlibrary.org/api/books?bibkeys={isbn}&format=json&jscmd=data");
            return JsonSerializer.Deserialize<Dictionary<string, OpenLibraryData>>(json);
        }

        private static async Task<Dictionary<string, OpenLibraryDetails>> GetDetails(string isbn)
        {
            var json = await client.GetStringAsync($"http://openlibrary.org/api/books?bibkeys={isbn}&format=json&jscmd=details");
            return JsonSerializer.Deserialize<Dictionary<string, OpenLibraryDetails>>(json);
        }
    }
}