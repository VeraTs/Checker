using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class Measurement
    {
        [Key]
        public string Type { get; set; }
    }
}
