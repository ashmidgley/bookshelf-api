using System.ComponentModel.DataAnnotations;

namespace Api
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
