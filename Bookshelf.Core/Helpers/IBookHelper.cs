namespace Bookshelf.Core
{
    public interface IBookHelper
    {
        BookDto ToBookDto(Book book);
        Book ToBook(BookDto dto);
    }
}