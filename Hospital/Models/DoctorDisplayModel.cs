using System;

namespace Hospital.Models
{
    public class DoctorDisplayModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public double Rating { get; set; }
        public string CareerInfo { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }

        public DoctorDisplayModel(int doctorId, string doctorName, 
            int departmentId, string departmentName,
            double rating, string careerInfo,
            string avatarUrl, string phoneNumber, 
            string mail)
        {
            DoctorId = doctorId;
            DoctorName = doctorName;
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            Rating = rating;
            CareerInfo = careerInfo;
            AvatarUrl = avatarUrl;
            PhoneNumber = phoneNumber;
            Mail = mail;
        }
    }
}
