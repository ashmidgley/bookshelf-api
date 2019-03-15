using System.Collections.Generic;
using System.Threading.Tasks;
using Reads.Models;

namespace Reads
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAll();
        Task<Book> Get(int id);
        void Add(Book book);
        void Update(Book book);
        void Delete(Book book);
    }
}
