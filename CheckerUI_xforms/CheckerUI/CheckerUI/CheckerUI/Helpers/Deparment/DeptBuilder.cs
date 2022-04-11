using System;
using System.Collections.Generic;
using System.Text;
using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers
{
    internal static class DeptBuilder
    {
        public static DeptModel  GenerateDept(string i_DeptName, int i_DeptID, int i_Maximum, List<Dish_item> i_dishes, eDeptState i_State)
        {
            var model = new DeptModel
            {
                m_DeptID = i_DeptID,
                m_DeptName = i_DeptName,
                m_MaximumParallelism = i_Maximum,
                m_DeptState = i_State,
                m_DishesList = new List<Dish_item>()
            };
            model.m_DishesList = i_dishes;

            return model;
        }
    }
}
