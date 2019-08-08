using System;
using System.ComponentModel.DataAnnotations;

namespace Bookshelf.Models 
{
    public class ApiKey 
    {
        [Key]
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Key { get; set; }
        public DateTime Created { get; set; }
    }
}