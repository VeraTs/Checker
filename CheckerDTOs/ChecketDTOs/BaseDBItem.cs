using System.ComponentModel.DataAnnotations;

namespace CheckerServer.Models
{
    public abstract class BaseDBItem
    {
        [Key]
        public int ID { get; set; }
    }
}
