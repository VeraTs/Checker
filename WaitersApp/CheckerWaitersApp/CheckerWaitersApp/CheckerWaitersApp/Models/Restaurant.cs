using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckerWaitersApp.Models
{
    public class Restaurant : BaseDBItem
    {
        [Required]
        public string name { get; set; }
        [Required]
        private string email { get; set; }
        [Required]
        private string password { get; set; }

        // fluff
        public string phone { get; set; }
        public string contactName { get; set; }
        public int balance { get; set; } = 0;

        // -------- Details of Physical representation of Restaurant --------

        // menus!
        public List<RestMenu> menus { get; set; } = new List<RestMenu>();

        // Line = Hot Line, Oven, Cold line, etc.
        public List<Line> lines { get; set; } = new List<Line>();

        // Serving areas
        public List<ServingArea> servingAreas { get; set; } = new List<ServingArea>();

    }
}
