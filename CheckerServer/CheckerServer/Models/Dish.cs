using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class Dish : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        [ForeignKey("Line")]
        public int LineId { get; set; }
        [ForeignKey("RestMenu")]
        public int RestMenuId { get; set; }

        public string Description { get; set; }
        public DishType Type { get; set; }
    }
}
