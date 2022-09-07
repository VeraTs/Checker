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
        public string name { get; set; }
        public int inStock { get; set; }
        public Measurement measurement { get; set; }
    }
}
