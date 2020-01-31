using System;

namespace Bookshelf.Core
{
    public class NewBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int RatingId { get; set; }
        public DateTime FinishedOn { get; set; }
    }
}