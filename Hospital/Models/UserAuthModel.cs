// <copyright file="UserAuthModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Models
{

    /// <summary>
    /// Blood types.
    /// </summary>
    public enum BloodType
    {
        /// <summary>
        /// A+ blood type.
        /// </summary>
        A_Positive,    // A+

        /// <summary>
        /// A- blood type.
        /// </summary>
        A_Negative,    // A-

        /// <summary>
        /// B+ blood type.
        /// </summary>
        B_Positive,    // B+

        /// <summary>
        /// B- blood type.
        /// </summary>
        B_Negative,    // B-

        /// <summary>
        /// AB+ blood type.
        /// </summary>
        AB_Positive,   // AB+

        /// <summary>
        /// AB- blood type.
        /// </summary>
        AB_Negative,   // AB-

        /// <summary>
        /// O+ blood type.
        /// </summary>
        O_Positive,    // O+

        /// <summary>
        /// O- blood type.
        /// </summary>
        O_Negative, // O-
    }

    /// <summary>
    /// User Model for Creating an Account.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserAuthModel"/> class.
    /// </remarks>
    /// <param name="userId">user id.</param>
    /// <param name="username"> user username.</param>
    /// <param name="password">user's password.</param>
    /// <param name="mail">user's mail.</param>
    /// <param name="role">user's role.</param>
    public class UserAuthModel(int userId, string username, string password, string mail, string role)
    {
        /// <summary>
        /// Create a default user (no ).
        /// </summary>
        public static readonly UserAuthModel Default = new UserAuthModel(0, "Guest", string.Empty, string.Empty, "User");

        /// <summary>
        /// Gets User ID.
        /// </summary>
        public int UserId { get; private set; } = userId;


        /// <summary>
        /// Gets user's Username.
        /// </summary>
        public string Username { get; private set; } = username;

        /// <summary>
        /// Gets user's Password.
        /// </summary>
        public string Password { get; private set; } = password;

        /// <summary>
        /// Gets user's Mail.
        /// </summary>
        public string Mail { get; private set; } = mail;

        /// <summary>
        /// Gets user's Role.
        /// </summary>
        public string Role { get; private set; } = role;


        /// <summary>
        /// Turn the user Model with the user's information into a string.
        /// </summary>
        /// <returns>a string with the user's informstion</returns>
        public override string ToString()
        {
            return this.UserId + this.Username + this.Password + this.Mail + this.Role;
        }
    }
}
