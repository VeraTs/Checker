using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerDTOs
{
    public abstract class BaseDBItem
    {
        [Key]
        public int ID { get; set; }
    }
}
