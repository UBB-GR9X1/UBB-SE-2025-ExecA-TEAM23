// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatientManagerModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the PatientManagerModel for managing patient data operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Hospital.DatabaseServices;
    using Hospital.Exceptions;
    using Hospital.Interfaces;
    using Hospital.Models;

    /// <summary>
    /// Manages patient data operations through the patient service.
    /// </summary>
    public class PatientManagerModel
    {
        private readonly IPatientService _patientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientManagerModel"/> class.
        /// </summary>
        /// <param name="patientService">The patient service to use for database operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when patientService is null.</exception>
        public PatientManagerModel(IPatientService patientService)
        {
            this._patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        /// <summary>
        /// Gets the current patient information for single patient operations.
        /// </summary>
        public PatientJointModel PatientInfo { get; private set; } = PatientJointModel.Default;

        /// <summary>
        /// Gets the list of patients for multi-patient operations.
        /// </summary>
        public List<PatientJointModel> PatientList { get; private set; } = new List<PatientJointModel>();

        /// <summary>
        /// Loads patient information for a specific user ID.
        /// </summary>
        /// <param name="userId">The user ID to load information for.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> LoadPatientInfoByUserId(int userId)
        {
            this.PatientInfo = await this._patientService.GetPatientByUserId(userId).ConfigureAwait(false);
            Debug.WriteLine($"Patient info loaded: {this.PatientInfo.PatientName}");
            return true;
        }

        /// <summary>
        /// Loads all patients from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> LoadAllPatients()
        {
            this.PatientList = await this._patientService.GetAllPatients().ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Updates the password for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="password">The new password.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the password is invalid.</exception>
        public async Task<bool> UpdatePassword(int userId, string password)
        {
            if (string.IsNullOrEmpty(password) || password.Contains(' '))
            {
                throw new InputProfileException("Invalid password!\nCan't be null or with space");
            }

            if (password.Length > 255)
            {
                throw new InputProfileException("Invalid password!\nCan't be more than 255 characters");
            }

            return await this._patientService.UpdatePassword(userId, password);
        }

        /// <summary>
        /// Updates the email for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="email">The new email address.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the email is invalid.</exception>
        public async Task<bool> UpdateEmail(int userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
            {
                throw new InputProfileException("Invalid email format.");
            }

            if (email.Length > 100)
            {
                throw new InputProfileException("Invalid mail\nCan't be more than 100 characters");
            }

            return await this._patientService.UpdateEmail(userId, email);
        }

        /// <summary>
        /// Updates the username for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="username">The new username.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the username is invalid.</exception>
        public async Task<bool> UpdateUsername(int userId, string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Contains(' '))
            {
                throw new InputProfileException("Invalid username!\nCan't be null or with space");
            }

            if (username.Length > 50)
            {
                throw new InputProfileException("Invalid username!\nCan't be more than 50 characters");
            }

            return await this._patientService.UpdateUsername(userId, username);
        }

        /// <summary>
        /// Updates the name for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="name">The new name.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the name is invalid.</exception>
        public async Task<bool> UpdateName(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit))
            {
                throw new InputProfileException("Name cannot be empty.");
            }

            if (name.Length > 100)
            {
                throw new InputProfileException("Invalid name!\nName has to be at most 100 characters long");
            }

            return await this._patientService.UpdateName(userId, name);
        }

        /// <summary>
        /// Updates the birth date for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="birthDate">The new birth date.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateBirthDate(int userId, DateOnly birthDate)
        {
            return await this._patientService.UpdateBirthDate(userId, birthDate);
        }

        /// <summary>
        /// Updates the address for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="address">The new address.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the address is invalid.</exception>
        public async Task<bool> UpdateAddress(int userId, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                address = string.Empty;
            }

            if (address.Length > 255)
            {
                throw new InputProfileException("Invalid address\nCan't be more than 255 characters");
            }

            return await this._patientService.UpdateAddress(userId, address);
        }

        /// <summary>
        /// Updates the phone number for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the phone number is invalid.</exception>
        public async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                throw new InputProfileException("Invalid phone number!\nIt must have length 10");
            }

            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    throw new InputProfileException("Invalid phone number!\nOnly numbers allowed");
                }
            }

            return await this._patientService.UpdatePhoneNumber(userId, phoneNumber);
        }

        /// <summary>
        /// Updates the emergency contact for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="emergencyContact">The new emergency contact.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the emergency contact is invalid.</exception>
        public async Task<bool> UpdateEmergencyContact(int userId, string emergencyContact)
        {
            if (emergencyContact.Length != 10)
            {
                throw new InputProfileException("Invalid emergency contact!\nIt must have length 10");
            }

            foreach (char c in emergencyContact)
            {
                if (!char.IsDigit(c))
                {
                    throw new InputProfileException("Invalid emergency contact!\nOnly numbers allowed");
                }
            }

            return await this._patientService.UpdateEmergencyContact(userId, emergencyContact);
        }

        /// <summary>
        /// Updates the weight for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="weight">The new weight in kilograms.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the weight is invalid.</exception>
        public async Task<bool> UpdateWeight(int userId, double weight)
        {
            if (weight <= 0)
            {
                throw new InputProfileException("Weight must be greater than 0");
            }

            return await this._patientService.UpdateWeight(userId, weight);
        }

        /// <summary>
        /// Updates the height for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="height">The new height in centimeters.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        /// <exception cref="InputProfileException">Thrown when the height is invalid.</exception>
        public async Task<bool> UpdateHeight(int userId, int height)
        {
            if (height <= 0)
            {
                throw new InputProfileException("Height must be greater than 0.");
            }

            return await this._patientService.UpdateHeight(userId, height);
        }

        /// <summary>
        /// Logs an update action for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to log the action for.</param>
        /// <param name="action">The type of action being logged.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await this._patientService.LogUpdate(userId, action);
        }
    }
}
