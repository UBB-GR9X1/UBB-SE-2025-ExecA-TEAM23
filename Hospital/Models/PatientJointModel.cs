using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class PatientJointModel
    {
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public string Allergies { get; set; }
        public float Weight { get; set; }
        public int Height { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Cnp { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }


        public PatientJointModel(int userId, int patientId, string patientName, string bloodType, string emergencyContact, string allergies, float weight, int height, string username, string password, string mail, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            UserId = userId;
            PatientId = patientId;
            BloodType = bloodType;
            EmergencyContact = emergencyContact;
            Allergies = allergies;
            Weight = weight;
            Height = height;
            PatientName = patientName;
            Username = username;
            Password = password;
            Mail = mail;
            BirthDate = birthDate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }
    }
}