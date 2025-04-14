// <copyright file="IAuthViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.ViewModels
{
    using System.Threading.Tasks;
    using Hospital.Managers;
    using Hospital.Models;

    /// <summary>
    /// Interface for AuthViewModel:
    /// The view model for Login and Create account.
    /// </summary>
    public interface IAuthViewModel
    {
        /// <summary>
        /// The service / model for creating an account / loging in.
        /// </summary>
        IAuthManagerModel AuthManagerModel_ { get; }

        /// <summary>
        /// Creates an accout for the user.
        /// </summary>
        /// <param name="modelForCreatingUserAccount">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>.</returns>
        Task CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);

        /// <summary>
        /// Logs the user in if the user exists and the password for the account is correct.
        /// </summary>
        /// <param name="username">The user's username (from input).</param>
        /// <param name="password">the user's password (from input).</param>
        /// <returns>.</returns>
        /// <exception cref="AuthenticationException">Checks if the user exists and if the password is correct / valid. If not 
        /// it throws an exception.</exception>
        Task Login(string username, string password);

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>.</returns>
        Task Logout();
    }
}