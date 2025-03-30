using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class UserAuthModel
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Mail { get; private set; }

        public static readonly UserAuthModel Default = new UserAuthModel(0, "Guest", "", "");

        public UserAuthModel(int userId, string username, string password, string mail)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Mail = mail;
        }

        override public string ToString()
        {
            return UserId + Username + Password + Mail;
        }
    }
}
