using System.Collections.Generic;
using CheckerUI.Enums;
using CheckerUI.Models;

namespace CheckerUI.Helpers.LinesHelpers
{
    internal static class LineBuilder
    {
        public static Line  GenerateLine(string i_LineName, int i_LineID, int i_Maximum, List<Dish> i_dishes, eLineState i_State)
        {
            var model = new Line
            {
                id = i_LineID,
                Name = i_LineName,
                Limit = i_Maximum,
                State = i_State,
                Dishes = i_dishes
        };
            return model;
        }
    }
}
