using System;
using System.Collections.Generic;
using System.Text;

namespace CheckerUI.Models
{
    public class Maker
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int LineId { get; set; } // every maker lives in a line
    }
}
