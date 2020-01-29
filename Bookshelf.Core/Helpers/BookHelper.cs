using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookshelf.Core
{
    public class BookHelper : IBookHelper
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _dataFetcherApiUrl = "http://host.docker.internal:5001/api";

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
            var data = await GetData(newBook.ISBN);

            return new Book
            {
                UserId = newBook.UserId,
                CategoryId = newBook.CategoryId,
                RatingId = newBook.RatingId,
                FinishedOn = newBook.FinishedOn,
                Title = data.Title,
                Author = data.Author,
                PageCount = data.PageCount,
                ImageUrl = data.ImageUrl,
                Summary = data.Summary
            };
        }

        private async Task<DataFetcherDto> GetData(string isbn)
        {
            var json = await _client.GetStringAsync($"{_dataFetcherApiUrl}/values?isbn={isbn}");
            return JsonSerializer.Deserialize<DataFetcherDto>(json);
        }
    }
}