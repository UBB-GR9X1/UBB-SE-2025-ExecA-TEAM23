// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthManagerModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the AuthManagerModel class for managing user authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Managers
{
    using System;
    using System.Threading.Tasks;
    using Hospital.DatabaseServices;
    using Hospital.Exceptions;
    using Hospital.Interfaces;
    using Hospital.Models;

    /// <summary>
    /// Model for handling user authentication operations.
    /// </summary>
    public class AuthManagerModel
    {
        private readonly ILoginService _loginService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthManagerModel"/> class.
        /// </summary>
        /// <param name="loginService">The login service interface.</param>
        public AuthManagerModel(ILoginService loginService)
        {
            this._loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        /// <summary>
        /// Gets the current user authentication information.
        /// </summary>
        /// <remarks>The setter is private and is used internally by the authentication methods.</remarks>
        public UserAuthModel _userInfo { get; private set; } = UserAuthModel.Default;

        /// <summary>
        /// Loads a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>True if the user was found, otherwise false.</returns>
        public async Task<bool> LoadUserByUsername(string username)
        {
            try
            {
                this._userInfo = await this._loginService.GetUserByUsername(username).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies if the provided password matches the stored password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <returns>True if the password is verified, otherwise false.</returns>
        public async Task<bool> VerifyPassword(string password)
        {
            if (this._userInfo == UserAuthModel.Default)
            {
                return false;
            }

            if (!this._userInfo.Password.Equals(password))
            {
                return false;
            }

            return await this.LogAction(ActionType.LOGIN);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>True if the logout was successful, otherwise false.</returns>
        /// <exception cref="AuthenticationException">Thrown when no user is logged in.</exception>
        public async Task<bool> Logout()
        {
            if (this._userInfo == UserAuthModel.Default)
            {
                throw new AuthenticationException("Not logged in");
            }

            bool result = await this.LogAction(ActionType.LOGOUT);

            if (result)
            {
                this._userInfo = UserAuthModel.Default;
            }

            return result;
        }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The model containing user account information.</param>
        /// <returns>True if the account was created successfully, otherwise false.</returns>
        /// <exception cref="AuthenticationException">Thrown when the account data is invalid.</exception>
        public async Task<bool> CreateAccount(UserCreateAccountModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Contains(' '))
            {
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");
            }

            if (model.Username.Length > 50)
            {
                throw new AuthenticationException("Invalid username!\nCan't be more than 50 characters");
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Contains(' '))
            {
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");
            }

            if (model.Password.Length > 255)
            {
                throw new AuthenticationException("Invalid password!\nCan't be more than 255 characters");
            }

            if (string.IsNullOrEmpty(model.Mail) || !model.Mail.Contains('@') || !model.Mail.Contains('.'))
            {
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");
            }

            if (model.Mail.Length > 100)
            {
                throw new AuthenticationException("Invalid mail!\nCan't be more than 100 characters");
            }

            if (string.IsNullOrEmpty(model.Name) || !model.Name.Contains(' '))
            {
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");
            }

            if (model.Name.Length > 100)
            {
                throw new AuthenticationException("Invalid name!\nCan't be more than 100 characters");
            }

            if (model.Cnp.Length != 13)
            {
                throw new AuthenticationException("Invalid CNP!\nHas to have length 13");
            }

            foreach (char c in model.Cnp)
            {
                if (!char.IsDigit(c))
                {
                    throw new AuthenticationException("Invalid CNP!\nOnly numbers allowed");
                }
            }

            if (model.EmergencyContact.Length != 10)
            {
                throw new AuthenticationException("Invalid emergency contact!\nIt must have length 10");
            }

            foreach (char c in model.EmergencyContact)
            {
                if (!char.IsDigit(c))
                {
                    throw new AuthenticationException("Invalid emergency contact!\nOnly numbers allowed");
                }
            }

            // check if model is at least 14 years old
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minValidDate = todayDate.AddYears(-14);
            if (model.BirthDate > minValidDate)
            {
                throw new AuthenticationException("Invalid Date\nPatient must be at least 14 years old!");
            }

            // check if valid gender
            switch (model.BirthDate.Year <= 1999)
            {
                case true:
                    {
                        if (model.Cnp[0] != '1' && model.Cnp[0] != '2')
                        {
                            throw new AuthenticationException("CNP gender is invalid");
                        }

                        break;
                    }

                case false:
                    {
                        if (model.Cnp[0] != '5' && model.Cnp[0] != '6')
                        {
                            throw new AuthenticationException("CNP gender is invalid");
                        }

                        break;
                    }
            }

            if (model.BirthDate.Year.ToString().Length != 4)
            {
                throw new AuthenticationException("CNP birth year errorYou may be old, but you surely aren't this old :)!");
            }

            // check if valid match between birth date and CNP birth date
            if (model.BirthDate.Year.ToString().Substring(2, 2) != model.Cnp.Substring(1, 2))
            {
                throw new AuthenticationException("Mismatch between Birth year and CNP birth year");
            }

            if (model.BirthDate.Month.ToString().Length == 1)
            {
                if (model.BirthDate.Month.ToString()[0] != model.Cnp[4] || model.Cnp[3] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Month and CNP birth month");
                }
            }
            else
            {
                if (model.BirthDate.Month.ToString() != model.Cnp.Substring(3, 2))
                {
                    throw new AuthenticationException("Mismatch between Birth Month and CNP birth month");
                }
            }

            if (model.BirthDate.Day.ToString().Length == 1)
            {
                if (model.BirthDate.Day.ToString()[0] != model.Cnp[6] || model.Cnp[5] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Day and CNP birth day");
                }
            }
            else
            {
                if (model.BirthDate.Day.ToString() != model.Cnp.Substring(5, 2))
                {
                    throw new AuthenticationException("Mismatch between Birth Day and CNP birth day");
                }
            }

            bool result = await this._loginService.CreateAccount(model);
            if (result)
            {
                if (await this.LoadUserByUsername(model.Username))
                {
                    await this.LogAction(ActionType.CREATE_ACCOUNT);
                    return await this.LogAction(ActionType.LOGIN);
                }
            }

            return result;
        }

        /// <summary>
        /// Logs an action performed by the user.
        /// </summary>
        /// <param name="action">The type of action to log.</param>
        /// <returns>True if the action was logged successfully, otherwise false.</returns>
        public async Task<bool> LogAction(ActionType action)
        {
            return await this._loginService.AuthenticationLogService(this._userInfo.UserId, action);
        }
    }
}
