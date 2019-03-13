using System.Collections.Generic;
using Reads.Models;

namespace Reads
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        Book Get(int id);
        void Add(Book book);
        void Update(Book book);
        void Delete(Book book);
    }
}
