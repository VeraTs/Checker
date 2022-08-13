using System.ComponentModel.DataAnnotations;

namespace CheckerWaitersApp.Models
{
    public class Ingredient : BaseDBItem
    {
        [Required]
        public string name { get; set; }
        public int inStock { get; set; }
        public Measurement measurement { get; set; }
    }
}
