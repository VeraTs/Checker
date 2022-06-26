using System.ComponentModel.DataAnnotations;

namespace CheckerWaitersApp.Models
{
    public abstract class BaseDBItem
    {
        [Key]
        public int id { get; set; }
    }
}
