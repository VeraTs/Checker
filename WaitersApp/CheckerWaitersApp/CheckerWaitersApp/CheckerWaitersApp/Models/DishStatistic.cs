using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerWaitersApp.Models
{
    public class DishStatistic : BaseDBItem
    {
        public Dish dish { get; set; }
       
        public int dishId { get; set; }
        
        public DateTime start { get; set; }
      
        public DateTime finish { get; set; }
    }
}
