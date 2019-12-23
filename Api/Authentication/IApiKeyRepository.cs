using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IApiKeyRepository
    {
        Task<IReadOnlyDictionary<string, ApiKey>> GetKeys();
    }
}
