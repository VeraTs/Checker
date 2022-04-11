using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Enums;

namespace CheckerUI.Models
{
    public class DeptModel
    {
        public string m_DeptName { get; set; }
        public int m_DeptID { get; set; }
        public int m_MaximumParallelism { get; set; }
        
        public List<Dish_item> m_DishesList { get; set; }

        public eDeptState m_DeptState { get; set; }

      


    }
}
