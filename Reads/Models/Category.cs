using System.ComponentModel.DataAnnotations;

namespace Reads.Models
{
    public class Category : Entity
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
