using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private BookshelfContext _context;

        public ApiKeyRepository(BookshelfContext context) 
        {
            _context = context;
        }

        public Task<IReadOnlyDictionary<string, ApiKey>> GetKeys()
        {
            var keys = _context.ApiKeys.ToList();
            IReadOnlyDictionary<string, ApiKey> readonlyDictionary = keys.ToDictionary(x => x.Key, x => x);
            return Task.FromResult(readonlyDictionary);
        }
    }
}