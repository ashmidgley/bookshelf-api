using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IRatingRepository
    {
        IEnumerable<Rating> GetUserRatings(int userId);
        Rating GetRating(int id);
        int Add(Rating Rating);
        void Update(Rating Rating);
        void Delete(int id);
    }
}
