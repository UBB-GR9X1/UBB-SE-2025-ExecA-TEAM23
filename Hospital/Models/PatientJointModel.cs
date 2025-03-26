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
        //TODO: Adaugati daca mai trebe ceva

        public PatientJointModel(int userId, int patientId, string patientName, string bloodType, string emergencyContact, string allergies, float weight, int height)
        {
            UserId = userId;
            PatientId = patientId;
            BloodType = bloodType;
            EmergencyContact = emergencyContact;
            Allergies = allergies;
            Weight = weight;
            Height = height;
            PatientName = patientName;
        }
    }
}