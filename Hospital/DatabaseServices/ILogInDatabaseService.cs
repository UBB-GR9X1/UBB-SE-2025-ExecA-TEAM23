// <copyright file="ILogInDatabaseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.DatabaseServices
{
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for LogInDatabaseService which does the following things:
    /// Makes the connection with the database in order to get information about the user
    /// useful for the login and for creating a new account.
    /// </summary>
    public interface ILogInDatabaseService
    {
        /// <summary>
        /// Checks the action the user makes, loging in or loging out and adds it to the database.
        /// </summary>
        /// <param name="userId">The id (unique) of the user we are checking.</param>
        /// <param name="actionType_loginORlogout">The acction the user makes: loging in / loging out.</param>
        /// <returns> 1 of the rows were modified.</returns>
        /// <exception cref="AuthenticationException">Throws exception if the type was not valid or if 
        /// there was a logger action error.</exception>
        Task<bool> AuthenticationLogService(int userId, ActionType actionType_loginORlogout);

        /// <summary>
        /// Creates a user account with the given information and adds it to the database.
        /// </summary>
        /// <param name="modelForCreatingUserAccount">The "model" for creating an account - domain.</param>
        /// <returns> 1 if the user account was created, 0 otherwise.</returns>
        /// <exception cref="AuthenticationException">Throws an exception if the user already exists
        /// or if there was a database error.</exception>
        Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);

        /// <summary>
        /// Gets a user's information from the database based on the username.
        /// </summary>
        /// <param name="username">The username of the user we are searching for.</param>
        /// <returns>The user of type UserAuthModel.</returns>
        /// <exception cref="AuthenticationException">Exception in case the username was not found in the table.</exception>
        Task<UserAuthModel> GetUserByUsername(string username);
    }
}