// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Admin.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the Admin model class for representing hospital administrators.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Models
{
    using System;

    /// <summary>
    /// Model class representing a hospital administrator.
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Admin"/> class.
        /// </summary>
        /// <param name="adminId">The administrator identifier.</param>
        /// <param name="userId">The user identifier associated with this administrator.</param>
        /// <param name="userName">The username for login.</param>
        /// <param name="password">The password for authentication.</param>
        /// <param name="mail">The email address.</param>
        /// <param name="name">The full name of the administrator.</param>
        /// <param name="birthDate">The birth date.</param>
        /// <param name="cnp">The CNP (Personal Numerical Code).</param>
        /// <param name="address">The residential address.</param>
        /// <param name="phoneNumber">The contact phone number.</param>
        /// <param name="registrationDate">The date when the administrator was registered.</param>
        public Admin(int adminId, int userId, string userName, string password, string mail, string name, DateOnly birthDate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            this.AdminId = adminId;
            this.UserId = userId;
            this.UserName = userName;
            this.Password = password;
            this.Mail = mail;
            this.Name = name;
            this.BirthDate = birthDate;
            this.Cnp = cnp;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.RegistrationDate = registrationDate;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the administrator.
        /// </summary>
        private int AdminId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier associated with this administrator.
        /// </summary>
        private int UserId { get; set; }

        /// <summary>
        /// Gets or sets the username for login.
        /// </summary>
        private string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        private string Mail { get; set; }

        /// <summary>
        /// Gets or sets the full name of the administrator.
        /// </summary>
        private string Name { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        private DateOnly BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the CNP (Personal Numerical Code).
        /// </summary>
        private string Cnp { get; set; }

        /// <summary>
        /// Gets or sets the residential address.
        /// </summary>
        private string Address { get; set; }

        /// <summary>
        /// Gets or sets the contact phone number.
        /// </summary>
        private string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the date when the administrator was registered in the system.
        /// </summary>
        private DateTime RegistrationDate { get; set; }
    }
}
