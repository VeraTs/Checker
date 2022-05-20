using System.ComponentModel.DataAnnotations;

namespace CheckerServer.Models
{
    public class Measurement
    {
        [Key]
        public string Type { get; set; }
    }
}
