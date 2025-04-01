using System;

namespace Hospital.Models
{
    class Admin
    {
        private int AdminId { get; set; }
        private int UserId { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private string Mail { get; set; }
        private string Name { get; set; }
        private DateOnly BirthDate { get; set; }
        private string Cnp { get; set; }
        private string Address { get; set; }
        private string PhoneNumber { get; set; }
        private DateTime RegistrationDate { get; set; }


        public Admin(int adminId, int userId, string userName, string password, string mail, string name, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            AdminId = adminId;
            UserId = userId;
            UserName = userName;
            Password = password;
            Mail = mail;
            Name = name;
            BirthDate = birthDate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }
    }
}
