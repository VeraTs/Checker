using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers.Line
{
    internal static class LineBuilder
    {
        public static LineModel  GenerateLine(string i_LineName, int i_LineID, int i_Maximum, List<Dish_item> i_dishes, eLineState i_State)
        {
            var model = new LineModel
            {
                m_LineID = i_LineID,
                m_LineName = i_LineName,
                m_MaximumParallelism = i_Maximum,
                m_LineState = i_State,
                m_DishesList = new List<Dish_item>()
            };
            model.m_DishesList = i_dishes;

            return model;
        }
    }
}
