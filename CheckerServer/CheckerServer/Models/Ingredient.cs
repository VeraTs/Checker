using System.ComponentModel.DataAnnotations;

namespace CheckerServer.Models
{
    public class Ingredient : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        public int InStock { get; set; }
        public Measurement Measurement { get; set; }
    }
}
