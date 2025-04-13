// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthService.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the IAuthService interface for authentication operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for authentication service operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username for authentication.</param>
        /// <param name="password">The password for authentication.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> Login(string username, string password);

        /// <summary>
        /// Logs out the current authenticated user.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> Logout();

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The user account creation model with user details.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> CreateAccount(UserCreateAccountModel model);

        /// <summary>
        /// Gets the role of the currently authenticated user.
        /// </summary>
        /// <returns>A string representing the user's role.</returns>
        string GetUserRole();

        /// <summary>
        /// Gets the ID of the currently authenticated user.
        /// </summary>
        /// <returns>An integer representing the user's ID.</returns>
        int GetUserId();
    }
}