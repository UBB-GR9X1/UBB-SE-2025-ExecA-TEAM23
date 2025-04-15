// <copyright file="Config.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Configs
{
    /// <summary>
    /// Configuration for the database.
    /// </summary>
    public class Config
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
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (configurationInstance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (Lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (configurationInstance == null)
                    {
                        configurationInstance = new Config();
                    }
                }
            }
            return configurationInstance;
        }

        // We'll use this property to prove that our Singleton really works.

        // Microsoft.Data.SqlClient uses Encrypted=true by default, so we need to add TrustServerCertificate=True
        // _databaseConnection = "Data Source={SERVER NAME};Initial Catalog={DATABASE_NAME};Integrated Security=True;TrustServerCertificate=True"
        // private string _databaseConnection = "Data Source=LAPTOP-ANDU\\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;TrustServerCertificate=True";
        // private string _databaseConnection = "Data Source=DESKTOP-B33HRLE;Initial Catalog=HospitalDB;Integrated Security=True;TrustServerCertificate=True";
        // private string _databaseConnection = "Data Source=DESKTOP-DK2UM26;Initial Catalog=HospitalApp;Integrated Security=True;TrustServerCertificate=True";

        // private string _databaseConnection = "Data Source=DESKTOP-5A6VJDA;Initial Catalog=HospitalDB;Integrated Security=True;TrustServerCertificate=True";

        private readonly string databaseConnection = "Data Source=ASUS-ANA\\SQLEXPRESS;Initial Catalog=HospitalApp;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public virtual string DatabaseConnection
        {
            get { return this.databaseConnection; }
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
