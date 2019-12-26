using System.Collections.Generic;

namespace Api
{
    public interface IBookRepository
    {
        BookDto Get(int id);
        IEnumerable<BookDto> GetAll();
        int Add(Book book);
        void Update(BookDto dto);
        void Delete(int id);
    }
}