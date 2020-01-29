using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookshelf.Core
{
    public interface IBookHelper
    {
        BookDto ToBookDto(Book book);
        Book ToBook(BookDto dto);
        Task<Book> PullOpenLibraryData(NewBookDto newBook);
    }
}