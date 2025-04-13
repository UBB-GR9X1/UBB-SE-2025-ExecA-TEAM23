// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthViewModel.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the AuthViewModel class for handling authentication operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.ViewModels
{
    using System.Threading.Tasks;
    using Hospital.Exceptions;
    using Hospital.Interfaces;
    using Hospital.Managers;
    using Hospital.Models;

    /// <summary>
    /// ViewModel for authentication services. Implements the <see cref="IAuthService"/> interface.
    /// </summary>
    public class AuthViewModel : IAuthService
    {
        private AuthManagerModel authManagerModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthViewModel"/> class.
        /// </summary>
        /// <param name="auth">The authentication manager model.</param>
        public AuthViewModel(AuthManagerModel auth)
        {
            this.authManagerModel = auth;
        }

        /// <summary>
        /// Gets the authentication manager model.
        /// </summary>
        public AuthManagerModel AuthManagerModel => this.authManagerModel;

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username to authenticate.</param>
        /// <param name="password">The password to authenticate.</param>
        /// <returns>A task representing the login operation that returns true if login is successful.</returns>
        /// <exception cref="AuthenticationException">Thrown when credentials are invalid.</exception>
        public async Task<bool> Login(string username, string password)
        {
            bool userExists = await this.authManagerModel.LoadUserByUsername(username);

            if (!userExists)
            {
                throw new AuthenticationException("Username doesn't exist!");
            }

            bool validPassword = await this.authManagerModel.VerifyPassword(password);

            if (!validPassword)
            {
                throw new AuthenticationException("Invalid password!");
            }

            return true;
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A task representing the logout operation that returns true if logout is successful.</returns>
        public async Task<bool> Logout()
        {
            return await this.authManagerModel.Logout();
        }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The user account model containing the details for account creation.</param>
        /// <returns>A task representing the account creation operation that returns true if creation is successful.</returns>
        public async Task<bool> CreateAccount(UserCreateAccountModel model)
        {
            return await this.authManagerModel.CreateAccount(model);
        }

        /// <summary>
        /// Gets the role of the current user.
        /// </summary>
        /// <returns>The role of the current user.</returns>
        public string GetUserRole()
        {
            return this.authManagerModel._userInfo.Role;
        }

        /// <summary>
        /// Gets the ID of the current user.
        /// </summary>
        /// <returns>The user ID of the current user.</returns>
        public int GetUserId()
        {
            return this.authManagerModel._userInfo.UserId;
        }
    }
}
