using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookshelf
{
    public interface IBookRepository
    {
        Task<int> Add(Book book);
        Task Delete(Book book);
        Task<Book> Get(int id);
        Task<List<Book>> GetAll();
        Task Update(Book book);
    }
}