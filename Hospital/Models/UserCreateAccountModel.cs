namespace Hospital.Models
{
    using System;

    /// <summary>
    /// Model - Domain for a user (when the user creates an account).
    /// </summary>
    public class UserCreateAccountModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreateAccountModel"/> class.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="password">user password.</param>
        /// <param name="mail">user mail.</param>
        /// <param name="name">user's name.</param>
        /// <param name="birthDate">user's birthdate.</param>
        /// <param name="cnp">user's cnp.</param>
        /// <param name="bloodType">user's blood type.</param>
        /// <param name="emergencyContact">user's emergency contact.</param>
        /// <param name="weight">user's weigth.</param>
        /// <param name="height">user's heigth.</param>
        public UserCreateAccountModel(string username, string password, string mail, string name, DateOnly birthDate, string cnp, BloodType bloodType, string emergencyContact, double weight, int height)
        {
            this.Username = username;
            this.Password = password;
            this.Mail = mail;
            this.Name = name;
            this.BirthDate = birthDate;
            this.Cnp = cnp;
            this.BloodType = bloodType;
            this.EmergencyContact = emergencyContact;
            this.Weight = weight;
            this.Height = height;
        }

        /// <summary>
        /// Gets username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets Mail.
        /// </summary>
        public string Mail { get; private set; }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets BirthDate.
        /// </summary>
        public DateOnly BirthDate { get; private set; }

        /// <summary>
        /// Gets CNP.
        /// </summary>
        public string Cnp { get; private set; }

        /// <summary>
        /// Gets Blood type.
        /// </summary>
        public BloodType BloodType { get; private set; }

        /// <summary>
        /// Gets emergency contact.
        /// </summary>
        public string EmergencyContact { get; private set; }

        /// <summary>
        /// Gets user's weigth.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Gets user's heigth.
        /// </summary>
        public int Height { get; private set; }

    }
}
