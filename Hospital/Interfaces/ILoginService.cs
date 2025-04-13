// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoginService.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the ILoginService interface for authentication operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for login service operations.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>A task representing the asynchronous operation with the user authentication model.</returns>
        Task<UserAuthModel> GetUserByUsername(string username);

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The user account creation model with user details.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> CreateAccount(UserCreateAccountModel model);

        /// <summary>
        /// Logs an authentication-related action in the system.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the action.</param>
        /// <param name="type">The type of authentication action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> AuthenticationLogService(int userId, ActionType type);
    }
}