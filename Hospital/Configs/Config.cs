// <copyright file="Config.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Configs
{
    /// <summary>
    /// Configuration for the database.
    /// </summary>
    public class Config : IConfigProvider
    {
        private static readonly object Lock = new object();
        private static Config? configurationInstance;

        private Config()
        {
        }

        /// <summary>
        /// Creates a singleton instance of the configuration.
        /// </summary>
        /// <returns>.</returns>
        public static Config GetInstance()
        {
            if (configurationInstance == null)
            {
                lock (Lock)
                {
                    if (configurationInstance == null)
                    {
                        configurationInstance = new Config();
                    }
                }
            }
            return configurationInstance;
        }

        private readonly string databaseConnection = "Data Source=DESKTOP-2KUEEF3;Initial Catalog=HospitalApp;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public virtual string DatabaseConnection
        {
            get { return this.databaseConnection; }
        }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        /// <returns>Database connection string</returns>
        public string GetDatabaseConnection()
        {
            return DatabaseConnection;
        }

        /// <summary>
        /// first patient ID.
        /// </summary>
        public int patientId = 1;

        /// <summary>
        /// first Doctor ID.
        /// </summary>
        public int doctorId = 1;

        /// <summary>
        /// the slot duration.
        /// </summary>
        public int SlotDuration = 30;
    }
}