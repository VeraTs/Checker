using System.Collections.Generic;


namespace CheckerDTOs
{
    public class Restaurant : BaseDBItem
    {
        public string Name { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }

        // fluff
        public string Phone { get; set; }
        public string ContactName { get; set; }

        // -------- Details of Physical representation of Restaurant --------

        // menus!
        public List<RestMenu> Menus { get; set; } = new List<RestMenu>();
        
        // Line = Hot Line, Oven, Cold line, etc.
        public List<Line> Lines { get; set; } = new List<Line>();

        // Serving areas
        public List<ServingArea> ServingAreas { get; set; } = new List<ServingArea>();
    }
}
