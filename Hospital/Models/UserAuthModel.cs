using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    
    public enum BloodType
    {
        A_Positive,    // A+
        A_Negative,    // A-
        B_Positive,    // B+
        B_Negative,    // B-
        AB_Positive,   // AB+
        AB_Negative,   // AB-
        O_Positive,    // O+
        O_Negative     // O-
    }

    public class UserAuthModel
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Mail { get; private set; }
        public string Role { get; private set; }

        public static readonly UserAuthModel Default = new UserAuthModel(0, "Guest", "", "", "User");

        public UserAuthModel(int userId, string username, string password, string mail, string role)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Mail = mail;
            Role = role;
        }

        override public string ToString()
        {
            return UserId + Username + Password + Mail + Role;
        }
    }
}
