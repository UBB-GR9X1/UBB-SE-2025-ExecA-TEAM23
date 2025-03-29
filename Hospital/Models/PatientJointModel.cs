using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class PatientJointModel
    {
        private int UserId { get; set; }
        private int PatientId { get; set; }
        private string PatientName { get; set; }
        private string BloodType { get; set; }
        private string EmergencyContact { get; set; }
        private string Allergies { get; set; }
        private float Weight { get; set; }
        private int Height { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Mail { get; set; }
        private DateOnly BirthDate { get; set; }
        private string Cnp { get; set; }
        private string Address { get; set; }
        private string PhoneNumber { get; set; }
        private DateTime RegistrationDate { get; set; }

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