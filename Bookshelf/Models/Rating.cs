using System.ComponentModel.DataAnnotations;

namespace Bookshelf.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
