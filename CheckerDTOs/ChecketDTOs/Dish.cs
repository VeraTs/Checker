using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
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
