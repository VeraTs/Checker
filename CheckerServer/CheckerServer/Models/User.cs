using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class User
    {

        public string userEmail { get; set; }
        public string userPassword { get; set; }
        
    }
}

