﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
 
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetEmail()
        {
            return Email;
        }

     }
}
