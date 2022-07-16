﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckerWaitersApp.Models
{
    public class Restaurant : BaseDBItem
    {
        [Required]
        public string Name { get; set; }
        [Required]
        private string Email { get; set; }
        [Required]
        private string Password { get; set; }

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