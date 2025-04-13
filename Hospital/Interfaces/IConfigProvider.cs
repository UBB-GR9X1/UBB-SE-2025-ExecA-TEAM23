// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigProvider.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the IConfigProvider interface for accessing configuration settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    /// <summary>
    /// Interface for providing configuration values to the application.
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Gets the database connection string from configuration.
        /// </summary>
        /// <returns>A string containing the database connection information.</returns>
        string GetDatabaseConnection();

        /// <summary>
        /// Gets the default patient ID from configuration.
        /// </summary>
        /// <returns>An integer representing the patient ID.</returns>
        int GetPatientId();

        /// <summary>
        /// Gets the default doctor ID from configuration.
        /// </summary>
        /// <returns>An integer representing the doctor ID.</returns>
        int GetDoctorId();

        /// <summary>
        /// Gets the default appointment slot duration from configuration.
        /// </summary>
        /// <returns>An integer representing the slot duration in minutes.</returns>
        int GetDefaultSlotDuration();
    }
}