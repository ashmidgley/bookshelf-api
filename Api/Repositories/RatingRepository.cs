using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class RatingRepository : IRatingRepository
    {
        private readonly BookshelfContext _context;

        public RatingRepository(BookshelfContext context)
        {
            _context = context;
        }

        public IEnumerable<Rating> GetAll()
        {
            return  _context.Ratings
                .ToList();
        }

        public Rating Get(int id)
        {
            return _context.Ratings
                .Single(r => r.Id == id);
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
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var rating = _context.Ratings
                .Single(r => r.Id == id);
            _context.Ratings.Remove(rating);
            _context.SaveChanges();
        }
    }
}
