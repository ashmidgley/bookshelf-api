namespace Api
{
    public interface IBookHelper
    {
        BookDto ToBookDto(Book book);
        Book ToBook(BookDto dto);
    }
}