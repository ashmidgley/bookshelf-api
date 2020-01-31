using System.Threading.Tasks;

namespace Bookshelf.DataFetcher
{
    public interface IFetchHelper
    {
        Task<GoogleBooksDto> PullGoogleBooksData(string title, string author);
        Task<IsbnDto> PullGoogleBooksData(string isbn);
        Task<IsbnDto> PullOpenLibraryData(string isbn);
    }
}