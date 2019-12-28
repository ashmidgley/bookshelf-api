using System.ComponentModel.DataAnnotations;

namespace Api
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
