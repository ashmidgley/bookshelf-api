using System.Threading.Tasks;

namespace Bookshelf.Core
{
    public interface ISearchHelper
    {
        Task<Book> PullGoogleBooksData(NewBookDto book);
    }
}