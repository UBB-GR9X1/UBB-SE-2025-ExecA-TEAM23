using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    class UserAuthModel
    {
        private int UserId { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Mail { get; set; }

        public UserAuthModel(int userId, string username, string password, string mail)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Mail = mail;
        }
    }
}
