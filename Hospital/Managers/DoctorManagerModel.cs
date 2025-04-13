// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorManagerModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the DoctorManagerModel for managing doctor-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.DatabaseServices;
    using Hospital.Interfaces;
    using Hospital.Models;

    /// <summary>
    /// Manager model for doctor-related operations.
    /// </summary>
    public class DoctorManagerModel
    {
        private readonly IDoctorService _doctorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorManagerModel"/> class.
        /// </summary>
        /// <param name="doctorService">The doctor service interface.</param>
        public DoctorManagerModel(IDoctorService doctorService)
        {
            this._doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        }

        /// <summary>
        /// Gets the doctor information loaded from the data source.
        /// </summary>
        public DoctorDisplayModel _doctorInfo { get; private set; } = DoctorDisplayModel.Default;

        /// <summary>
        /// Gets or sets the list of doctors for search operations.
        /// </summary>
        public List<DoctorDisplayModel> _doctorList { get; set; } = new List<DoctorDisplayModel>();

        /// <summary>
        /// Gets the list of doctors in a specific department.
        /// </summary>
        /// <param name="departmentId">The department ID to filter by.</param>
        /// <returns>A list of doctor models belonging to the specified department.</returns>
        public async Task<List<DoctorJointModel>> GetDoctorsByDepartment(int departmentId)
        {
            return await this._doctorService.GetDoctorsByDepartment(departmentId);
        }

        /// <summary>
        /// Gets all doctors in the system.
        /// </summary>
        /// <returns>A list of all doctor models.</returns>
        public async Task<List<DoctorJointModel>> GetAllDoctors()
        {
            return await this._doctorService.GetAllDoctors();
        }

        /// <summary>
        /// Loads doctor information by user ID.
        /// </summary>
        /// <param name="doctorId">The doctor's user ID.</param>
        /// <returns>True if the doctor was found, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when there is an error loading the doctor information.</exception>
        public async Task<bool> LoadDoctorInfoByUserId(int doctorId)
        {
            try
            {
                this._doctorInfo = await this._doctorService.GetDoctorById(doctorId);

                if (this._doctorInfo != DoctorDisplayModel.Default)
                {
                    return true;
                }

                throw new Exception($"No doctor found for user ID: {doctorId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading doctor info: {ex.Message}");
            }
        }

        /// <summary>
        /// Searches for doctors by department name.
        /// </summary>
        /// <param name="departmentPartialName">The partial department name to search for.</param>
        /// <returns>True if doctors were found, otherwise false.</returns>
        public async Task<bool> SearchDoctorsByDepartment(string departmentPartialName)
        {
            this._doctorList = await this._doctorService.GetDoctorsByDepartmentPartialName(departmentPartialName);
            return this._doctorList != null;
        }

        /// <summary>
        /// Searches for doctors by name.
        /// </summary>
        /// <param name="namePartial">The partial doctor name to search for.</param>
        /// <returns>True if doctors were found, otherwise false.</returns>
        public async Task<bool> SearchDoctorsByName(string namePartial)
        {
            this._doctorList = await this._doctorService.GetDoctorsByPartialDoctorName(namePartial);
            return this._doctorList != null;
        }

        /// <summary>
        /// Updates a doctor's name.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="name">The new name.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when the name is invalid.</exception>
        public async Task<bool> UpdateDoctorName(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !name.Contains(' '))
            {
                throw new Exception("Doctor name cannot be empty and has to contain space");
            }

            if (name.Length > 100)
            {
                throw new Exception("Doctor name is too long");
            }

            return await this._doctorService.UpdateDoctorName(userId, name);
        }

        /// <summary>
        /// Updates a doctor's department.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="departmentId">The new department ID.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateDepartment(int userId, int departmentId)
        {
            return await this._doctorService.UpdateDoctorDepartment(userId, departmentId);
        }

        /// <summary>
        /// Updates a doctor's rating.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="rating">The new rating.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when the rating is invalid.</exception>
        public async Task<bool> UpdateRating(int userId, double rating)
        {
            if (rating < 0.0 || rating > 5.0)
            {
                throw new Exception("Rating must be between 0 and 5");
            }

            return await this._doctorService.UpdateDoctorRating(userId, rating);
        }

        /// <summary>
        /// Updates a doctor's career information.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="careerInfo">The new career information.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when the career info is too long.</exception>
        public async Task<bool> UpdateCareerInfo(int userId, string careerInfo)
        {
            if (careerInfo != null && careerInfo.Length > int.MaxValue)
            {
                throw new Exception("Career info is too long");
            }

            careerInfo ??= string.Empty;

            return await this._doctorService.UpdateDoctorCareerInfo(userId, careerInfo);
        }

        /// <summary>
        /// Updates a doctor's avatar URL.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="avatarUrl">The new avatar URL.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when the avatar URL is too long.</exception>
        public async Task<bool> UpdateAvatarUrl(int userId, string avatarUrl)
        {
            if (avatarUrl != null)
            {
                if (avatarUrl.Length > 255)
                {
                    throw new Exception("Avatar URL is too long");
                }
            }

            avatarUrl ??= string.Empty;

            return await this._doctorService.UpdateDoctorAvatarUrl(userId, avatarUrl);
        }

        /// <summary>
        /// Updates a doctor's phone number.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when the phone number is invalid.</exception>
        public async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            if (phoneNumber != null)
            {
                if (phoneNumber.Length != 10)
                {
                    throw new Exception("Phone number must have length 10");
                }

                foreach (char c in phoneNumber)
                {
                    if (!char.IsDigit(c))
                    {
                        throw new Exception("Phone numbers must contain only digits");
                    }
                }
            }

            phoneNumber ??= string.Empty;

            return await this._doctorService.UpdateDoctorPhoneNumber(userId, phoneNumber);
        }

        /// <summary>
        /// Updates a doctor's email address.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="email">The new email address.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        /// <exception cref="Exception">Thrown when the email is invalid.</exception>
        public async Task<bool> UpdateEmail(int userId, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email cannot be empty");
            }

            if (email.Length > 100)
            {
                throw new Exception("Email is too long");
            }

            if (!email.Contains('@') || !email.Contains('.'))
            {
                throw new Exception("Invalid email format!\nNeeds to have @ and .");
            }

            return await this._doctorService.UpdateDoctorEmail(userId, email);
        }

        /// <summary>
        /// Logs an update action for the doctor.
        /// </summary>
        /// <param name="userId">The doctor's user ID.</param>
        /// <param name="action">The action type to log.</param>
        /// <returns>True if the logging was successful, otherwise false.</returns>
        public async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await this._doctorService.UpdateLogService(userId, action);
        }
    }
}
