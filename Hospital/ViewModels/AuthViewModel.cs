// <copyright file="AuthViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.ViewModels
{
    using System.Threading.Tasks;
    using Hospital.Exceptions;
    using Hospital.Managers;
    using Hospital.Models;


    /// <summary>
    /// The view model for Login and Create account.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuthViewModel"/> class.
    /// </remarks>
    /// <param name="userServiceModel">Servuce for Login or Create Account.</param>
    public class AuthViewModel(IAuthManagerModel userServiceModel) : IAuthViewModel
    {
        /// <summary>
        /// Gets the Service (Model) for the user.
        /// </summary>
        public IAuthManagerModel AuthManagerModel_ { get; private set; } = userServiceModel;

        /// <summary>
        /// Logs the user in if the user exists and the password for the account is correct.
        /// </summary>
        /// <param name="username">The user's username (from input).</param>
        /// <param name="password">the user's password (from input).</param>
        /// <returns>.</returns>
        /// <exception cref="AuthenticationException">Checks if the user exists and if the password is correct / valid. If not 
        /// it throws an exception.</exception>
        public async Task Login(string username, string password)
        {
            bool checkIfUserExists = await this.AuthManagerModel_.LoadUserByUsername(username);

            if (!checkIfUserExists)
            {
                throw new AuthenticationException("Username doesn't exist!");
            }

            bool isThePasswordValid = await this.AuthManagerModel_.VerifyPassword(password);

            if (!isThePasswordValid)
            {
                throw new AuthenticationException("Invalid password!");
            }
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>.</returns>
        public async Task Logout()
        {
            await this.AuthManagerModel_.Logout();
        }

        /// <summary>
        /// Creates an accout for the user.
        /// </summary>
        /// <param name="modelForCreatingUserAccount">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>.</returns>
        public async Task CreateAccount(UserCreateAccountModel modelForCreatingUserAccount)
        {
            await this.AuthManagerModel_.CreateAccount(modelForCreatingUserAccount);
        }

        /// <summary>
        /// Gets the user's role.
        /// </summary>
        /// <returns>user's role.</returns>
        public string GetUserRole()
        {
            return userServiceModel.allUserInformation.Role;
        }
    }
}
