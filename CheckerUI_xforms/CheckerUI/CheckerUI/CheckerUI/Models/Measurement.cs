using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CheckerUI.Models
{
    public class Measurement
    {
        [Key]
        public string Type { get; set; }
    }
}
