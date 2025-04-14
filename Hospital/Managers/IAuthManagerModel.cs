// <copyright file="IAuthManagerModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Managers
{
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for IAuthManagerModel:
    /// </summary>
    public interface IAuthManagerModel
    {

        /// <summary>
        /// Gets the user information (given as UserAuthModel).
        /// </summary>
        UserAuthModel allUserInformation { get; }

        /// <summary>
        /// Creates an accout, checking for validation errors in the user's inputs.
        /// </summary>
        /// <param name="modelForCreatingUserAccount">The user's information Model given as UserCreateAccountModel</param>
        /// <returns>User action: LOGIN if the accout got created, LOGOUT otherwise.</returns>
        /// <exception cref="AuthenticationException">Exceptions if the inputs were not valid
        /// + messages for each validation error.</exception>
        Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);

        /// <summary>
        /// Loads the user page based on the username.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <returns>true, no mather what.</returns>
        Task<bool> LoadUserByUsername(string username);

        /// <summary>
        /// Sets the user's action.
        /// </summary>
        /// <param name="actionType_loginORlogout">The type of the user's action.</param>
        /// <returns>The action setter.</returns>
        Task<bool> LogAction(ActionType actionType_loginORlogout);

        /// <summary>
        /// Logs out the user from the application (goes back to Main Window).
        /// </summary>
        /// <returns>the result of the logging out</returns>
        /// <exception cref="AuthenticationException">Checks if the user is logged in, throws an exception if not.</exception>
        Task<bool> Logout();

        /// <summary>
        /// Checks if the password matches at log in (if the user typed the right password).
        /// </summary>
        /// <param name="userInputPassword">The user input password.</param>
        /// <returns>true, if the password matches with the user's one, or false if the one from the input does not match.</returns>
        Task<bool> VerifyPassword(string userInputPassword);
    }
}