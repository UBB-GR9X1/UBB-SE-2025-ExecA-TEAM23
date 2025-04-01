using System;

namespace Hospital.Models
{
    public class DoctorDisplayModel
    {
        public int DoctorId { get; private set; }
        public string DoctorName { get; private set; }
        public int DepartmentId { get; private set; }
        public string DepartmentName { get; private set; }
        public double Rating { get; private set; }
        public string CareerInfo { get; private set; }
        public string AvatarUrl { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Mail { get; private set; }

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
            AvatarUrl = "https://picsum.photos/200";
            PhoneNumber = phoneNumber;
            Mail = mail;
        }
    }
}
