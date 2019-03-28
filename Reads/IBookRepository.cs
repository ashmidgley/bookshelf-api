using System.Collections.Generic;
using System.Threading.Tasks;
using Reads.Models;

namespace Reads
{
    public interface IBookRepository
    {
        void Add(Book book);
        void Delete(Book book);
        Task<Book> Get(int id);
        Task<List<Book>> GetAll();
        void Update(Book book);
    }
}