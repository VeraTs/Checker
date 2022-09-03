using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Ingredient : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        public int InStock { get; set; }
        public Measurement Measurement { get; set; }
    }
}
