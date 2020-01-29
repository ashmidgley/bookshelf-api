using System;

namespace Bookshelf.Core
{
    public class NewBookDto
    {
        public string ISBN { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int RatingId { get; set; }
        public DateTime FinishedOn { get; set; }
    }
}