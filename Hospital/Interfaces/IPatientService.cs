// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPatientService.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the IPatientService interface for patient-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for patient-related operations and data management.
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Gets all patients in the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of patient joint models.</returns>
        Task<List<PatientJointModel>> GetAllPatients();

        /// <summary>
        /// Gets a patient by their user ID.
        /// </summary>
        /// <param name="userId">The user ID of the patient to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with the patient joint model.</returns>
        Task<PatientJointModel> GetPatientByUserId(int userId);

        /// <summary>
        /// Updates a patient's password.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="password">The new password.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdatePassword(int userId, string password);

        /// <summary>
        /// Updates a patient's email address.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="email">The new email address.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateEmail(int userId, string email);

        /// <summary>
        /// Updates a patient's username.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="username">The new username.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateUsername(int userId, string username);

        /// <summary>
        /// Updates a patient's name.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="name">The new name.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateName(int userId, string name);

        /// <summary>
        /// Updates a patient's birth date.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="birthDate">The new birth date.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateBirthDate(int userId, DateOnly birthDate);

        /// <summary>
        /// Updates a patient's address.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="address">The new address.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateAddress(int userId, string address);

        /// <summary>
        /// Updates a patient's phone number.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdatePhoneNumber(int userId, string phoneNumber);

        /// <summary>
        /// Updates a patient's emergency contact information.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="emergencyContact">The new emergency contact information.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateEmergencyContact(int userId, string emergencyContact);

        /// <summary>
        /// Updates a patient's weight.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="weight">The new weight in kilograms.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateWeight(int userId, double weight);

        /// <summary>
        /// Updates a patient's height.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="height">The new height in centimeters.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateHeight(int userId, int height);

        /// <summary>
        /// Logs an update action performed by or for a patient.
        /// </summary>
        /// <param name="userId">The user ID of the patient.</param>
        /// <param name="actionType">The type of update action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> LogUpdate(int userId, ActionType actionType);
    }
}
