using System.ComponentModel.DataAnnotations;

namespace CheckerWaitersApp.Models
{
    public class Measurement
    {
        [Key]
        public string type { get; set; }
    }
}
