// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDoctorService.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the IDoctorService interface for doctor-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for doctor-related operations and data management.
    /// </summary>
    public interface IDoctorService
    {
        /// <summary>
        /// Gets a doctor by user ID.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <returns>A task representing the asynchronous operation with the doctor display model.</returns>
        Task<DoctorDisplayModel> GetDoctorById(int userId);

        /// <summary>
        /// Gets all doctors.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of doctor joint models.</returns>
        Task<List<DoctorJointModel>> GetAllDoctors();

        /// <summary>
        /// Gets doctors by partial department name.
        /// </summary>
        /// <param name="departmentPartialName">The partial name of the department to search for.</param>
        /// <returns>A task representing the asynchronous operation with a list of doctor display models.</returns>
        Task<List<DoctorDisplayModel>> GetDoctorsByDepartmentPartialName(string departmentPartialName);

        /// <summary>
        /// Gets doctors by department ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>A task representing the asynchronous operation with a list of doctor joint models.</returns>
        Task<List<DoctorJointModel>> GetDoctorsByDepartment(int departmentId);

        /// <summary>
        /// Gets doctors by partial name match.
        /// </summary>
        /// <param name="doctorPartialName">The partial name of the doctor to search for.</param>
        /// <returns>A task representing the asynchronous operation with a list of doctor display models.</returns>
        Task<List<DoctorDisplayModel>> GetDoctorsByPartialDoctorName(string doctorPartialName);

        /// <summary>
        /// Updates a doctor's name.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newName">The new name to set.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorName(int userId, string newName);

        /// <summary>
        /// Updates a doctor's department.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newDepartmentId">The new department ID to assign.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorDepartment(int userId, int newDepartmentId);

        /// <summary>
        /// Updates a doctor's rating.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newRating">The new rating value.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorRating(int userId, double newRating);

        /// <summary>
        /// Updates a doctor's career information.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newCareerInfo">The new career information text.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorCareerInfo(int userId, string newCareerInfo);

        /// <summary>
        /// Updates a doctor's avatar URL.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newAvatarUrl">The new avatar URL.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorAvatarUrl(int userId, string newAvatarUrl);

        /// <summary>
        /// Updates a doctor's phone number.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newPhoneNumber">The new phone number.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorPhoneNumber(int userId, string newPhoneNumber);

        /// <summary>
        /// Updates a doctor's email address.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="newEmail">The new email address.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateDoctorEmail(int userId, string newEmail);

        /// <summary>
        /// Logs an action performed by a doctor in the system.
        /// </summary>
        /// <param name="userId">The user ID of the doctor.</param>
        /// <param name="actionType">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> UpdateLogService(int userId, ActionType actionType);
    }
}
