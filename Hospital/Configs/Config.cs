// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the Config class that provides application configuration values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Configs
{
    using Hospital.Interfaces;

    /// <summary>
    /// Singleton class that provides configuration values for the application.
    /// Implements the IConfigProvider interface.
    /// </summary>
    public class Config : IConfigProvider
    {
        /// <summary>
        /// The default appointment slot duration in minutes.
        /// </summary>
        private const int DefaultSlotDuration = 30;

        /// <summary>
        /// The default patient ID for testing or initial setup.
        /// </summary>
        private const int DefaultPatientId = 1;

        /// <summary>
        /// The default doctor ID for testing or initial setup.
        /// </summary>
        private const int DefaultDoctorId = 1;

        /// <summary>
        /// Lock object for thread-safe singleton instantiation.
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// The singleton instance of the Config class.
        /// </summary>
        private static Config? instance;

        /// <summary>
        /// The database connection string.
        /// </summary>
        private readonly string databaseConnection = "Data Source=DESKTOP-2KUEEF3;Initial Catalog=HospitalApp;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Prevents a default instance of the <see cref="Config"/> class from being created.
        /// </summary>
        private Config()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the Config class.
        /// </summary>
        /// <returns>The singleton instance of Config.</returns>
        public static Config GetInstance()
        {
            if (instance == null)
            {
                lock (LockObject)
                {
                    if (instance == null)
                    {
                        instance = new Config();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <returns>A string containing the database connection information.</returns>
        public string GetDatabaseConnection()
        {
            return this.databaseConnection;
        }

        /// <summary>
        /// Gets the default patient ID from configuration.
        /// </summary>
        /// <returns>An integer representing the patient ID.</returns>
        public int GetPatientId()
        {
            return DefaultPatientId;
        }

        /// <summary>
        /// Gets the default doctor ID from configuration.
        /// </summary>
        /// <returns>An integer representing the doctor ID.</returns>
        public int GetDoctorId()
        {
            return DefaultDoctorId;
        }

        /// <summary>
        /// Gets the default appointment slot duration from configuration.
        /// </summary>
        /// <returns>An integer representing the slot duration in minutes.</returns>
        public int GetDefaultSlotDuration()
        {
            return DefaultSlotDuration;
        }
    }
}
