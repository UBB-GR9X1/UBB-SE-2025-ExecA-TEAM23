// <copyright file="AuthManagerModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Managers
{
    using System;
    using System.Threading.Tasks;
    using Hospital.DatabaseServices;
    using Hospital.Exceptions;
    using Hospital.Models;
    using Windows.Services.Maps;

    /// <summary>
    /// Service for the Login / Create Account.
    /// </summary>
    public class AuthManagerModel : IAuthManagerModel
    {
        private readonly ILogInDatabaseService logInDatabaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthManagerModel"/> class.
        /// </summary>
        /// <param name="databaseService">.</param>
        public AuthManagerModel(ILogInDatabaseService databaseService)
        {
            this.logInDatabaseService = databaseService;
        }

        /// <summary>
        /// Validation Numbers, Limits and Digits for Creating an Account.
        /// </summary>
        public enum NumbersForValidationsWhenCreatingAnAccount
        {
            /// <summary>
            /// The maximum Limit for the Username Length.
            /// </summary>
            LimitForUsernameLength = 50,

            /// <summary>
            /// The maximum Limit for the Password Length.
            /// </summary>
            LimitForPasswordLength = 255,

            /// <summary>
            /// The maximum Limit for the Mail Length.
            /// </summary>
            LimitForMailLength = 100,

            /// <summary>
            /// The maximum Limit for the Name Length.
            /// </summary>
            LimitForNamelLength = 100,

            /// <summary>
            /// The maximum Limit for the CNP Length.
            /// </summary>
            LimitForCNPLength = 13,

            /// <summary>
            /// The maximum Limit for the Emergency Contact Length.
            /// </summary>
            LimitForEmergencyContactLength = 10,

            /// <summary>
            /// The minimum age the user should have for creating an account is 14 years old.
            /// </summary>
            MinimumAgeForUser = -14,

            /// <summary>
            /// The first digit-index from the CNP.
            /// </summary>
            FirstDigitFromTheCNP = 0,

            /// <summary>
            /// Limit for the Birthdate Year Length.
            /// </summary>
            LimitForBirthdateYearLength = 4,

            /// <summary>
            /// The age for which the first digit of the cnp changes its values.
            /// </summary>
            AgeForChangingFirstDigitOfTheCNP = 1999,

            /// <summary>
            /// Limit for the Birthdate Month Length.
            /// </summary>
            LimitForBirthdateMonthLength = 1,

            /// <summary>
            /// The second digit-index from the CNP / birthdate.
            /// </summary>
            SecondDigitFromCnpOrBirthdate = 1,

            /// <summary>
            /// The third digit-index from the CNP / birthdate.
            /// </summary>
            ThirdDigitFromBirthdateORCNP = 2,

            /// <summary>
            /// The fourth digit-index from the Birthdate.
            /// </summary>
            FourthDigitFromBirthdate = 3,

            /// <summary>
            /// The fourth digit-index from the CNP.
            /// </summary>
            FourthDigitFromCNP = 3,

            /// <summary>
            /// The first digit-index from the Birthdate.
            /// </summary>
            FirstDigitFromBirthDate = 0,

            /// <summary>
            /// The sixth digit-index from the CNP.
            /// </summary>
            SixthDigitOfTheCNP = 5,

            /// <summary>
            /// The fifth digit-index from the CNP.
            /// </summary>
            FifthDigitOfTheCNP = 4,

            /// <summary>
            /// The seventh digit-index from the CNP.
            /// </summary>
            SeventhDigitOfTheCNP = 6,

        }

        /// <summary>
        /// Gets the user information (given as UserAuthModel).
        /// </summary>
        public UserAuthModel allUserInformation { get; private set; } = UserAuthModel.Default;

        /// <summary>
        /// Loads the user page based on the username.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <returns>true, no mather what.</returns>
        public async Task<bool> LoadUserByUsername(string username)
        {
            this.allUserInformation = await this.logInDatabaseService.GetUserByUsername(username).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Checks if the password matches at log in (if the user typed the right password).
        /// </summary>
        /// <param name="userInputPassword">The user input password.</param>
        /// <returns>true, if the password matches with the user's one, or false if the one from the input does not match.</returns>
        public async Task<bool> VerifyPassword(string userInputPassword)
        {
            if (this.allUserInformation == UserAuthModel.Default)
            {
                return false;
            }

            if (!this.allUserInformation.Password.Equals(userInputPassword))
            {
                return false;
            }

            return await this.LogAction(ActionType.LOGIN);
        }

        /// <summary>
        /// Logs out the user from the application (goes back to Main Window).
        /// </summary>
        /// <returns>the result of the logging out</returns>
        /// <exception cref="AuthenticationException">Checks if the user is logged in, throws an exception if not.</exception>
        public async Task<bool> Logout()
        {
            if (this.allUserInformation == UserAuthModel.Default)
            {
                throw new AuthenticationException("Not logged in");
            }

            bool resultForLoggingout = await this.LogAction(ActionType.LOGOUT);

            if (resultForLoggingout)
            {
                this.allUserInformation = UserAuthModel.Default;
            }

            return resultForLoggingout;
        }

        /// <summary>
        /// Creates an accout, checking for validation errors in the user's inputs.
        /// </summary>
        /// <param name="modelForCreatingUserAccount">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>User action: LOGIN if the accout got created, LOGOUT otherwise.</returns>
        /// <exception cref="AuthenticationException">Exceptions if the inputs were not valid
        /// + messages for each validation error.</exception>
        public async Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount)
        {
            if (string.IsNullOrWhiteSpace(modelForCreatingUserAccount.Username) || modelForCreatingUserAccount.Username.Contains(' '))
            {
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");
            }

            if (modelForCreatingUserAccount.Username.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LimitForUsernameLength)
            {
                throw new AuthenticationException("Invalid username!\nCan't be more than 50 characters");
            }

            if (string.IsNullOrEmpty(modelForCreatingUserAccount.Password) || modelForCreatingUserAccount.Password.Contains(' '))
            {
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");
            }

            if (modelForCreatingUserAccount.Password.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LimitForPasswordLength)
            {
                throw new AuthenticationException("Invalid password!\nCan't be more than 255 characters");
            }

            if (string.IsNullOrEmpty(modelForCreatingUserAccount.Mail) || !modelForCreatingUserAccount.Mail.Contains('@') || !modelForCreatingUserAccount.Mail.Contains('.'))
            {
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");
            }

            if (modelForCreatingUserAccount.Mail.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LimitForMailLength)
            {
                throw new AuthenticationException("Invalid mail!\nCan't be more than 100 characters");
            }

            if (string.IsNullOrEmpty(modelForCreatingUserAccount.Name) || !modelForCreatingUserAccount.Name.Contains(' '))
            {
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");
            }

            if (modelForCreatingUserAccount.Name.Length > (int)NumbersForValidationsWhenCreatingAnAccount.LimitForNamelLength)
            {
                throw new AuthenticationException("Invalid name!\nCan't be more than 100 characters");
            }

            if (modelForCreatingUserAccount.Cnp.Length != (int)NumbersForValidationsWhenCreatingAnAccount.LimitForCNPLength)
            {
                throw new AuthenticationException("Invalid CNP!\nHas to have length 13");
            }

            foreach (char characterFromCNP in modelForCreatingUserAccount.Cnp)
            {
                if (!char.IsDigit(characterFromCNP))
                {
                    throw new AuthenticationException("Invalid CNP!\nOnly numbers allowed");
                }
            }

            if (modelForCreatingUserAccount.EmergencyContact.Length != (int)NumbersForValidationsWhenCreatingAnAccount.LimitForEmergencyContactLength)
            {
                throw new AuthenticationException("Invalid emergency contact!\nIt must have length 10");
            }

            foreach (char oneCharacterFromEmergencyContact in modelForCreatingUserAccount.EmergencyContact)
            {
                if (!char.IsDigit(oneCharacterFromEmergencyContact))
                {
                    throw new AuthenticationException("Invalid emergency contact!\nOnly numbers allowed");
                }
            }

            // check if model is at least 14 years old
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minValidDate = todayDate.AddYears((int)NumbersForValidationsWhenCreatingAnAccount.MinimumAgeForUser);
            if (modelForCreatingUserAccount.BirthDate > minValidDate)
            {
                throw new AuthenticationException("Invalid Date\nPatient must be at least 14 years old!");
            }

            // check if valid gender
            switch (modelForCreatingUserAccount.BirthDate.Year <= (int)NumbersForValidationsWhenCreatingAnAccount.AgeForChangingFirstDigitOfTheCNP)
            {
                case true:
                    if (modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FirstDigitFromTheCNP] != '1' && modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FirstDigitFromTheCNP] != '2')
                    {
                        throw new AuthenticationException("CNP gender is invalid");
                    }

                    break;
                case false:
                    if (modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FirstDigitFromTheCNP] != '5' && modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FirstDigitFromTheCNP] != '6')
                    {
                        throw new AuthenticationException("CNP gender is invalid");
                    }

                    break;
            }

            if (modelForCreatingUserAccount.BirthDate.Year.ToString().Length != (int)NumbersForValidationsWhenCreatingAnAccount.LimitForBirthdateYearLength)
            {
                throw new AuthenticationException("CNP birth year errorYou may be old, but you surely aren't this old :)!");
            }

            // check if valid match between birth date and CNP birth date
            if (modelForCreatingUserAccount.BirthDate.Year.ToString().Substring((int)NumbersForValidationsWhenCreatingAnAccount.ThirdDigitFromBirthdateORCNP, (int)NumbersForValidationsWhenCreatingAnAccount.ThirdDigitFromBirthdateORCNP) 
                != modelForCreatingUserAccount.Cnp.Substring((int)NumbersForValidationsWhenCreatingAnAccount.SecondDigitFromCnpOrBirthdate, (int)NumbersForValidationsWhenCreatingAnAccount.ThirdDigitFromBirthdateORCNP))
            {
                throw new AuthenticationException("Mismatch between Birth year and CNP birth year");
            }

            if (modelForCreatingUserAccount.BirthDate.Month.ToString().Length == (int)NumbersForValidationsWhenCreatingAnAccount.LimitForBirthdateMonthLength)
            {
                if (modelForCreatingUserAccount.BirthDate.Month.ToString()[(int)NumbersForValidationsWhenCreatingAnAccount.FirstDigitFromBirthDate]
                    != modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FifthDigitOfTheCNP]
                    || modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.FourthDigitFromCNP] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Month and CNP birth month");
                }
            }
            else
                if (modelForCreatingUserAccount.BirthDate.Month.ToString() != 
                modelForCreatingUserAccount.Cnp.Substring((int)NumbersForValidationsWhenCreatingAnAccount.FourthDigitFromBirthdate, (int)NumbersForValidationsWhenCreatingAnAccount.ThirdDigitFromBirthdateORCNP))
            {
                throw new AuthenticationException("Mismatch between Birth Month and CNP birth month");
            }

            if (modelForCreatingUserAccount.BirthDate.Day.ToString().Length == (int)NumbersForValidationsWhenCreatingAnAccount.LimitForBirthdateMonthLength)
            {
                if (modelForCreatingUserAccount.BirthDate.Day.ToString()[(int)NumbersForValidationsWhenCreatingAnAccount.FirstDigitFromBirthDate] != modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.SeventhDigitOfTheCNP] || modelForCreatingUserAccount.Cnp[(int)NumbersForValidationsWhenCreatingAnAccount.SixthDigitOfTheCNP] != '0')
                {
                    throw new AuthenticationException("Mismatch between Birth Day and CNP birth day");
                }
            }
            else
                if (modelForCreatingUserAccount.BirthDate.Day.ToString() != modelForCreatingUserAccount.Cnp.Substring((int)NumbersForValidationsWhenCreatingAnAccount.SixthDigitOfTheCNP, (int)NumbersForValidationsWhenCreatingAnAccount.ThirdDigitFromBirthdateORCNP))
            {
                throw new AuthenticationException("Mismatch between Birth Day and CNP birth day");
            }

            bool result = await this.logInDatabaseService.CreateAccount(modelForCreatingUserAccount);
            if (result)
            {
                if (await this.LoadUserByUsername(modelForCreatingUserAccount.Username))
                {
                    return await this.LogAction(ActionType.LOGIN);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the user's action.
        /// </summary>
        /// <param name="actionType_loginORlogout">The type of the user's action.</param>
        /// <returns>The action setter.</returns>
        public async Task<bool> LogAction(ActionType actionType_loginORlogout)
        {
            return await this.logInDatabaseService.AuthenticationLogService(this.allUserInformation.UserId, actionType_loginORlogout);
        }
    }
}
