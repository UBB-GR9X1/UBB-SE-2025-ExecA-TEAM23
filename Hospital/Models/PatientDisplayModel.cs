using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    class PatientDisplayModel
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string CNP { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContactName { get; set; }
        public string Allergy { get; set; }
        public float Weight { get; set; }
        public int Height { get; set; }
        public DateTime RegistrationDate { get; set; }

        public PatientDisplayModel(
        int patientId, string name, string cnp, string username, string mail, string password,
        DateOnly birthdate, string address, string phoneNumber, string bloodType,
        string emergencyContactName, string allergy, float weight, int height, DateTime registrationDate)
        {
            PatientId = patientId;
            Name = name;
            CNP = cnp;
            Username = username;
            Mail = mail;
            Password = password;
            Birthdate = birthdate;
            Address = address;
            PhoneNumber = phoneNumber;
            BloodType = bloodType;
            EmergencyContactName = emergencyContactName;
            Allergy = allergy;
            Weight = weight;
            Height = height;
            RegistrationDate = registrationDate;
        }
    }
}
