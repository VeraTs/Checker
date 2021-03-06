using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class DishStatistic : BaseDBItem
    {
        
        public Dish Dish { get; set; }
        [ForeignKey("Dish")]
        public int DishId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime Finish { get; set; }
    }
}
