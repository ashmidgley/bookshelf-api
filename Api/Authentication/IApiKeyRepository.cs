using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookshelf
{
    public interface IApiKeyRepository
    {
        Task<IReadOnlyDictionary<string, ApiKey>> GetKeys();
    }
}
