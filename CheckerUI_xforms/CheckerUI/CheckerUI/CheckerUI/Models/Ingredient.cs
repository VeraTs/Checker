using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text;

namespace CheckerUI.Models
{
    public class Ingredient : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        public int InStock { get; set; }
        public Measurement Measurement { get; set; }
    }
}
