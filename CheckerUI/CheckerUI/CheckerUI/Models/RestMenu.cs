using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CheckerUI.Models
{
    public class RestMenu : BaseDBItem
    {
       
        public int restaurantId { get; set; }
        
        public string name { get; set; }

        // practicals
        public List<Dish> dishes { get; set; } = new List<Dish>();
    }
}
