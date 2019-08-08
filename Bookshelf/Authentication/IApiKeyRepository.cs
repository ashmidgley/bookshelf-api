using System.Collections.Generic;
using System.Threading.Tasks;
using Bookshelf.Models;

namespace Bookshelf.Authentication {
    public interface IApiKeyRepository {
        Task<IReadOnlyDictionary<string, ApiKey>> GetKeys();
    }
}
