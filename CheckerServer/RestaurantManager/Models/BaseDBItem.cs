using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public abstract class BaseDBItem
    {
        [Key]
        public int ID { get; set; }
    }
}
