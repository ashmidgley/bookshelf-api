using System.Collections.Generic;
using System.Threading.Tasks;
using Bookshelf.Models;

namespace Bookshelf
{
    public interface IBookRepository
    {
        int Add(Book book);
        void Delete(Book book);
        Task<Book> Get(int id);
        Task<List<Book>> GetAll();
        void Update(Book book);
    }
}