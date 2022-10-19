using System.ComponentModel.DataAnnotations;

namespace CheckerServer.Models
{
    public class Restaurant : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        // fluff
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public int Balance { get; set; } = 0;

        // -------- Details of Physical representation of Restaurant --------

        // menus!
        public List<RestMenu> Menus { get; set; } = new List<RestMenu>();
        
        // Line = Hot Line, Oven, Cold line, etc.
        public List<Line> Lines { get; set; } = new List<Line>();

        // Serving areas
        public List<ServingArea> ServingAreas { get; set; } = new List<ServingArea>();
        
    }
}
