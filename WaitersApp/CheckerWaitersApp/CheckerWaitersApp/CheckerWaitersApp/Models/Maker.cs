using System;
using System.Collections.Generic;
using System.Text;

namespace CheckerWaitersApp.Models
{
    public class Maker : BaseDBItem
    {
        public string name { get; set; }
        public int lineId { get; set; } // every maker lives in a line
    }
}
