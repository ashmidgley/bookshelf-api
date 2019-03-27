using System;
using System.ComponentModel.DataAnnotations;

namespace Reads.Models
{
    public class Book : Entity
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public DateTime StartedOn { get; set; }
        [Required]
        public DateTime FinishedOn { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        public string Summary { get; set; }
        public bool Removed { get; set; }
    }
}
