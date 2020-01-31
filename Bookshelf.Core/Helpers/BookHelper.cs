namespace Bookshelf.Core
{
    public class BookHelper : IBookHelper
    {
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
    }
}