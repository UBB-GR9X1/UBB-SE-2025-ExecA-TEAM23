namespace Hospital.Models
{
    using System;

    /// <summary>
    /// Model for the user from the Dashboard.
    /// </summary>
    class User
    {
        /// <summary>
        /// Gets or Sets the user ID.
        /// </summary>
        private int UserId { get; set; }

        /// <summary>
        /// Gets or Sets user's username.
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// Gets or Sets the user's password.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Gets or Sets user's mail.
        /// </summary>
        private string Mail { get; set; }

        /// <summary>
        /// Gets or Sets user's role.
        /// </summary>
        private string Role { get; set; }

        /// <summary>
        /// Gets or Sets user's name.
        /// </summary>
        private string Name { get; set; }

        /// <summary>
        /// Gets or Sets user's birthdate.
        /// </summary>
        private DateOnly BirthDate { get; set; }

        /// <summary>
        /// Gets or Sets user's CNP.
        /// </summary>
        private string Cnp { get; set; }

        /// <summary>
        /// Gets or Sets user's address.
        /// </summary>
        private string Address { get; set; }

        /// <summary>
        /// Gets or Sets user's phone number.
        /// </summary>
        private string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or Sets the registration time.
        /// </summary>
        private DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <param name="username">username.</param>
        /// <param name="password">password.</param>
        /// <param name="mail">mail.</param>
        /// <param name="role">role.</param>
        /// <param name="name">name.</param>
        /// <param name="birthDate">birthdate.</param>
        /// <param name="cnp">cnp.</param>
        /// <param name="address">address.</param>
        /// <param name="phoneNumber">phone number.</param>
        /// <param name="registrationDate">registration date.</param>
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
