namespace Api
{
    public class BookHelper : IBookHelper
    {
        public BookDto ToBookDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                CategoryId = book.CategoryId,
                RatingId = book.RatingId,
                Image = book.Image,
                Title = book.Title,
                Author = book.Author,
                StartedOn = book.StartedOn,
                FinishedOn = book.FinishedOn,
                Year = book.FinishedOn.Year,
                PageCount = book.PageCount,
                Summary = book.Summary,
                Removed = book.Removed
            };
        }

        public Book ToBook(BookDto dto)
        {
            return new Book
            {
                Id = dto.Id,
                CategoryId = dto.CategoryId,
                RatingId = dto.RatingId,
                Image = dto.Image,
                Title = dto.Title,
                Author = dto.Author,
                StartedOn = dto.StartedOn,
                FinishedOn = dto.FinishedOn,
                PageCount = dto.PageCount,
                Summary = dto.Summary,
                Removed = dto.Removed
            };
        }
    }
}