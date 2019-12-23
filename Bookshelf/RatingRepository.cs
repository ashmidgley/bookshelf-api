using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf
{
    public class RatingRepository : IRatingRepository
    {
        private readonly BookshelfContext _context;

        public RatingRepository(BookshelfContext context)
        {
            _context = context;
        }

        public async Task<List<Rating>> GetAll()
        {
            return await _context.Ratings
                .ToListAsync();
        }

        public async Task<Rating> Get(int id)
        {
            return await _context.Ratings
                .SingleAsync(r => r.Id == id);
        }

        public async Task<int> Add(Rating Rating)
        {
            _context.Ratings.Add(Rating);
            await _context.SaveChangesAsync();
            return Rating.Id;
        }

        public async Task Update(Rating Rating)
        {
            _context.Ratings.Update(Rating);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Rating Rating)
        {
            _context.Ratings.Remove(Rating);
            await _context.SaveChangesAsync();
        }
    }
}
