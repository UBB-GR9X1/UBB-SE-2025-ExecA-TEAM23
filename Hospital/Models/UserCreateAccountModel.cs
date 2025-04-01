using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class UserCreateAccountModel
    {
        public UserCreateAccountModel(string username, string password, string mail, string name, DateOnly birthDate, string cnp, BloodType bloodType, string emergencyContact, double weight, int height)
        {
            Username = username;
            Password = password;
            Mail = mail;
            Name = name;
            BirthDate = birthDate;
            Cnp = cnp;
            BloodType = bloodType;
            EmergencyContact = emergencyContact;
            Weight = weight;
            Height = height;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Mail { get; private set; }
        public string Name { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string Cnp { get; private set; }
        public BloodType BloodType { get; private set; }
        public string EmergencyContact { get; private set; }
        public double Weight { get; private set; }
        public int Height { get; private set; }

    }
}
