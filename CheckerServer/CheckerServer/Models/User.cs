using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class User
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }
}

