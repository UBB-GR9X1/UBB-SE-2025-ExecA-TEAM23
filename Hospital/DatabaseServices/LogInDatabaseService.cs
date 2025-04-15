// <copyright file="LogInDatabaseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.DatabaseServices
{
    using System;
    using System.Threading.Tasks;
    using Hospital.Configs;
    using Hospital.Exceptions;
    using Hospital.Models;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Makes the connection with the database in order to get information about the user
    /// useful for the login and for creating a new account.
    /// </summary>
    public class LogInDatabaseService : ILogInDatabaseService
    {
        private readonly Config databaseConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInDatabaseService"/> class.
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public LogInDatabaseService()
        {
            this.databaseConfiguration = Config.GetInstance();
        }


        /// <summary>
        /// The column positions of the user's information (indexes for the columns).
        /// </summary>
        public enum UserDatabaseFieldColumnPositions
        {
            /// <summary>
            /// The unique identifier of the user (column index 0).
            /// </summary>
            UserId = 0,

            /// <summary>
            /// The username of the user (column index 1).
            /// </summary>
            UserName = 1,

            /// <summary>
            /// The password of the user (column index 2).
            /// </summary>
            UserPassword = 2,

            /// <summary>
            /// The email address of the user (column index 3).
            /// </summary>
            UserMail = 3,

            /// <summary>
            /// The role of the user, indicating whether they are a patient or a doctor (column index 4).
            /// </summary>
            UserRole_Pacient_or_Doctor = 4,
        }

        /// <summary>
        /// Gets a user's information from the database based on the username.
        /// </summary>
        /// <param name="username">The username of the user we are searching for.</param>
        /// <returns>The user of type UserAuthModel.</returns>
        /// <exception cref="AuthenticationException">Exception in case the username was not found in the table.</exception>
        public async Task<UserAuthModel> GetUserByUsername(string username)
        {
            const string queryToGetUsersByTheGivenUsername = "SELECT UserId, Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber, RegistrationDate FROM Users U WHERE U.Username = @username";

            using SqlConnection connectionToTheDatabase = new SqlConnection(this.databaseConfiguration.DatabaseConnection);

            await connectionToTheDatabase.OpenAsync().ConfigureAwait(false);

            using SqlCommand selectCommand = new SqlCommand(queryToGetUsersByTheGivenUsername, connectionToTheDatabase);

            selectCommand.Parameters.AddWithValue("@username", username);

            SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

            if (await reader.ReadAsync().ConfigureAwait(false))
            {
                int userId = reader.GetInt32((int)UserDatabaseFieldColumnPositions.UserId);
                string userName = reader.GetString((int)UserDatabaseFieldColumnPositions.UserName);
                string password = reader.GetString((int)UserDatabaseFieldColumnPositions.UserPassword);
                string mail = reader.GetString((int)UserDatabaseFieldColumnPositions.UserMail);
                string role = reader.GetString((int)UserDatabaseFieldColumnPositions.UserRole_Pacient_or_Doctor);

                connectionToTheDatabase.Close();
                return new UserAuthModel(userId, userName, password, mail, role);
            }
            else
            {
                connectionToTheDatabase.Close();
                throw new AuthenticationException("No user found with given username");
            }
        }

        /// <summary>
        /// Creates a user account with the given information and adds it to the database.
        /// </summary>
        /// <param name="modelForCreatingUserAccount">The "model" for creating an account - domain.</param>
        /// <returns> 1 if the user account was created.</returns>
        /// <exception cref="AuthenticationException">Throws an exception if the user already exists
        /// or if there was a database error.</exception>
        public async Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount)
        {
            string? bloodType = null;
            switch (modelForCreatingUserAccount.BloodType)
            {
                case BloodType.A_Positive:
                    bloodType = "A+";
                    break;
                case BloodType.A_Negative:
                    bloodType = "A-";
                    break;
                case BloodType.B_Positive:
                    bloodType = "B+";
                    break;
                case BloodType.B_Negative:
                    bloodType = "B-";
                    break;
                case BloodType.AB_Positive:
                    bloodType = "AB+";
                    break;
                case BloodType.AB_Negative:
                    bloodType = "AB-";
                    break;
                case BloodType.O_Positive:
                    bloodType = "O+";
                    break;
                case BloodType.O_Negative:
                    bloodType = "O-";
                    break;
            }

            using SqlConnection connectionToDatabase = new SqlConnection(this.databaseConfiguration.DatabaseConnection);
            await connectionToDatabase.OpenAsync().ConfigureAwait(false);

            using SqlTransaction transaction = connectionToDatabase.BeginTransaction();
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username OR Mail = @mail OR Cnp = @cnp;";
                using SqlCommand checkCommand = new SqlCommand(checkQuery, connectionToDatabase, transaction);
                checkCommand.Parameters.AddWithValue("@username", modelForCreatingUserAccount.Username);
                checkCommand.Parameters.AddWithValue("@mail", modelForCreatingUserAccount.Mail);
                checkCommand.Parameters.AddWithValue("@cnp", modelForCreatingUserAccount.Cnp);

                object? resultCheck = await checkCommand.ExecuteScalarAsync().ConfigureAwait(false);
                if (resultCheck != null && Convert.ToInt32(resultCheck) > 0)
                {
                    throw new AuthenticationException("User already exists!");
                }

                string insertUserQuery = "INSERT INTO Users(Username, Password, Mail, Role, Name, BirthDate, Cnp) OUTPUT INSERTED.UserId VALUES" +
                "(@username, @password, @mail, @role, @name, @birthDate, @cnp)";

                using SqlCommand userCommand = new SqlCommand(insertUserQuery, connectionToDatabase, transaction);
                userCommand.Parameters.AddWithValue("@username", modelForCreatingUserAccount.Username);
                userCommand.Parameters.AddWithValue("@password", modelForCreatingUserAccount.Password);
                userCommand.Parameters.AddWithValue("@mail", modelForCreatingUserAccount.Mail);
                userCommand.Parameters.AddWithValue("@name", modelForCreatingUserAccount.Name);
                userCommand.Parameters.AddWithValue("@role", "Patient");
                userCommand.Parameters.AddWithValue("@birthDate", modelForCreatingUserAccount.BirthDate);
                userCommand.Parameters.AddWithValue("@cnp", modelForCreatingUserAccount.Cnp);

                object? userIdObj = await userCommand.ExecuteScalarAsync().ConfigureAwait(false);
                if (userIdObj == null)
                {
                    throw new Exception("User insertion failed.");
                }

                int userId = Convert.ToInt32(userIdObj);

                string insertPatientQuery = @"
                INSERT INTO Patients (UserId, BloodType, EmergencyContact, Weight, Height)
                VALUES (@userId, @bloodType, @emergencyContact, @weight, @height);";

                using SqlCommand patientCommand = new SqlCommand(insertPatientQuery, connectionToDatabase, transaction);
                patientCommand.Parameters.AddWithValue("@userId", userId);
                if (bloodType != null)
                {
                    patientCommand.Parameters.AddWithValue("@bloodType", bloodType);
                }

                patientCommand.Parameters.AddWithValue("@emergencyContact", modelForCreatingUserAccount.EmergencyContact);
                patientCommand.Parameters.AddWithValue("@weight", modelForCreatingUserAccount.Weight);
                patientCommand.Parameters.AddWithValue("@height", modelForCreatingUserAccount.Height);

                int patientResult = await patientCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                transaction.Commit();

                return patientResult == 1;
            }
            catch (AuthenticationException errror_AuthentificationException)
            {
                throw new AuthenticationException(errror_AuthentificationException.Message);
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new AuthenticationException("Database error " + exception.Message);
            }
        }

        /// <summary>
        /// Checks the action the user makes, loging in or loging out and adds it to the database.
        /// </summary>
        /// <param name="userId">The id (unique) of the user we are checking.</param>
        /// <param name="actionType_loginORlogout">The acction the user makes: loging in / loging out.</param>
        /// <returns> 1 of the rows were modified.</returns>
        /// <exception cref="AuthenticationException">Throws exception if the type was not valid or if 
        /// there was a logger action error.</exception>
        public async Task<bool> AuthenticationLogService(int userId, ActionType actionType_loginORlogout)
        {
            string query = "INSERT INTO Logs (UserId, ActionType) VALUES (@userId, @type)";
            try
            {
                using SqlConnection connectionToDatabase = new SqlConnection(this.databaseConfiguration.DatabaseConnection);

                await connectionToDatabase.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand(query, connectionToDatabase);

                command.Parameters.AddWithValue("@userId", userId);

                switch (actionType_loginORlogout)
                {
                    case ActionType.LOGIN:
                        command.Parameters.AddWithValue("@type", "LOGIN");
                        break;
                    case ActionType.LOGOUT:
                        command.Parameters.AddWithValue("@type", "LOGOUT");
                        break;
                    case ActionType.CREATE_ACCOUNT:
                        command.Parameters.AddWithValue("@type", "CREATE_ACCOUNT");
                        break;
                    default:
                        throw new AuthenticationException("Invalid type for Authentication Log");
                }

                int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                connectionToDatabase.Close();

                return rowsAffected == 1;

            }
            catch (SqlException)
            {
                throw new AuthenticationException("Error Action Logger");
            }
        }
    }
}
