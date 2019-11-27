using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api
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
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public int Add(Rating Rating)
        {
            _context.Ratings.Add(Rating);
            _context.SaveChanges();
            return Rating.Id;
        }

        public void Update(Rating Rating)
        {
            _context.Ratings.Update(Rating);
            _context.SaveChangesAsync();
        }

        public void Delete(Rating Rating)
        {
            _context.Ratings.Remove(Rating);
            _context.SaveChangesAsync();
        }
    }
}
