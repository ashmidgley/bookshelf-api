using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookshelf.Core
{
    public interface ISearchHelper
    {
        Task<IEnumerable<Book>> SearchBooks(string title, string author, int maxResults);
    }
}