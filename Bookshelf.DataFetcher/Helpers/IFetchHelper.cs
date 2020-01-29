using System.Threading.Tasks;

namespace Bookshelf.DataFetcher
{
    public interface IFetchHelper
    {
        Task<BookDto> PullOpenLibraryData(string isbn);
    }
}