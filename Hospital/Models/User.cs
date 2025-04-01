using System;

namespace Hospital.Models
{
    class User
    {
        private int UserId { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Mail { get; set; }
        private string Role { get; set; }
        private string Name { get; set; }
        private DateOnly BirthDate { get; set; }
        private string Cnp { get; set; }
        private string Address { get; set; }
        private string PhoneNumber { get; set; }
        private DateTime RegistrationDate { get; set; }

        public User(int userId, string username, string password, string mail, string role, string name, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Mail = mail;
            Role = role;
            Name = name;
            BirthDate = birthDate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }
    }
}
