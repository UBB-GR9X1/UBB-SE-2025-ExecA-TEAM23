using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class DoctorJointModel
    {
        private int DoctorId { get; set; }
        private int UserId { get; set; }
        private int DepartmentId { get; set; }
        private double Rating { get; set; }
        private string LicenseNumber { get; set; }
        private string DoctorName { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Mail { get; set; }
        private DateOnly BirthDate { get; set; }
        private string Cnp { get; set; }
        private string Address { get; set; }
        private string PhoneNumber { get; set; }
        private DateTime RegistrationDate { get; set; }

        public DoctorJointModel(int doctorId, int userId, string doctorName, int departmentId, double rating, string licenseNumber, string username, string password, string mail, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            DoctorId = doctorId;
            UserId = userId;
            DepartmentId = departmentId;
            Rating = rating;
            LicenseNumber = licenseNumber;
            DoctorName = doctorName;
            Username = username;
            Password = password;
            Mail = mail;
            BirthDate = birthDate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }

        // E okey folosesc gettere publice?
        public string GetDoctorName()
        {
            return DoctorName;
        }
        public double GetDoctorRating()
        {
            return Rating;
        }
    }
}