using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientSampler.TDOs
{
    internal class ToDoList : List<ToDo>
    {
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append("To Do List:\n");
            res.Append("-------------------------------------\n");
            foreach (ToDo i_toDo in this)
            {
                res.Append(i_toDo.ToString());
            }

            return res.ToString();
        }
    }
}
