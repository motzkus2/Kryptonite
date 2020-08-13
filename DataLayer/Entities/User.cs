using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Salt { get; set; }

        public string HashedPassword { get; set; }
        

    }
}
