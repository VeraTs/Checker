using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CheckerUI.Models
{
    public abstract class BaseDBItem
    {
        [Key]
        public int id { get; set; }
    }
}
