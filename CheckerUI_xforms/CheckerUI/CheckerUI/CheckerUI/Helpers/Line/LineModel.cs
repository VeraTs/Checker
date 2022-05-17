using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers.Line
{
    public class LineModel
    {
        public string m_LineName { get; set; }
        public int m_LineID { get; set; }
        public int m_MaximumParallelism { get; set; }
        
        public List<Dish_item> m_DishesList { get; set; }

        public eLineState m_LineState { get; set; }

    }
}
