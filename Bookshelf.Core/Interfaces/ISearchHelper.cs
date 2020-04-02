using System.Threading.Tasks;

namespace Bookshelf.Core
{
    public interface ISearchHelper
    {
        Task<bool> BookExists(NewBookDto book);
        Task<Book> PullBook(NewBookDto book);
    }
}