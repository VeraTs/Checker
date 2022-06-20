using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class DishStatistic
    {
        public int ID;
        public Dish Dish { get; set; }
        public int DishId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}
