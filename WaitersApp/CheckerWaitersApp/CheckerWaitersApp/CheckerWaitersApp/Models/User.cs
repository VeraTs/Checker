using System;
using System.Collections.Generic;
using System.Text;

namespace CheckerWaitersApp.Models
{
    public class User
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public User(string email, string password)
        {
            UserEmail = email;
            UserPassword = password;
        }
        public User() { }
    }
}
