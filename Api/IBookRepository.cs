using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
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