using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IBookHelper
    {
        BookDto ToBookDto(Book book);
        Book ToBook(BookDto dto);
        Task<Book> PullOpenLibraryData(NewBookDto newBook);
    }
}