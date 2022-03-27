using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientSampler.TDOs
{
    internal class ToDo
    {
        public int id { get; set; }
        public String description { get; set; }
        public String createdDate { get; set; }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append("\tid : " + id+"\n");
            res.Append("\tdesc : " + description + "\n");
            res.Append("\tcreated: " + createdDate.Substring(0, createdDate.IndexOf('T')) + "\n");

            return res.ToString();

        }
    }
}
