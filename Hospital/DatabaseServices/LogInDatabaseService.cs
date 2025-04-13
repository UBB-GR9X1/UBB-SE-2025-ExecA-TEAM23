using Hospital.Configs;
using Hospital.Exceptions;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace Hospital.DatabaseServices
{
    public class LogInDatabaseService : ILogInDatabaseService
    {
        private readonly Config _config;

        public LogInDatabaseService()
        {
            _config = Config.GetInstance();
        }

        public async Task<UserAuthModel> GetUserByUsername(string username)
        {
            const string query = "SELECT UserId, Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber, RegistrationDate FROM Users U WHERE U.Username = @username";

            using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);

            // Open the database connection asynchronously
            await connection.OpenAsync().ConfigureAwait(false);

            using SqlCommand selectCommand = new SqlCommand(query, connection);

            selectCommand.Parameters.AddWithValue("@username", username);

            SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

            if (await reader.ReadAsync().ConfigureAwait(false))
            {
                int userId = reader.GetInt32(0);
                string userName = reader.GetString(1);
                string password = reader.GetString(2);
                string mail = reader.GetString(3);
                string role = reader.GetString(4);
                connection.Close();
                return new UserAuthModel(userId, userName, password, mail, role);
            }
            connection.Close();
            throw new AuthenticationException("No user found with given username");
        }

        public async Task<bool> CreateAccount(UserCreateAccountModel model)
        {
            string? bloodType = null;
            switch (model.BloodType)
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
            using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
            await connection.OpenAsync().ConfigureAwait(false);

            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username OR Mail = @mail OR Cnp = @cnp;";
                using SqlCommand checkCommand = new SqlCommand(checkQuery, connection, transaction);
                checkCommand.Parameters.AddWithValue("@username", model.Username);
                checkCommand.Parameters.AddWithValue("@mail", model.Mail);
                checkCommand.Parameters.AddWithValue("@cnp", model.Cnp);

                object? resultCheck = await checkCommand.ExecuteScalarAsync().ConfigureAwait(false);
                if (resultCheck != null && Convert.ToInt32(resultCheck) > 0)
                    throw new AuthenticationException("User already exists!");

                string insertUserQuery = "INSERT INTO Users(Username, Password, Mail, Role, Name, BirthDate, Cnp) OUTPUT INSERTED.UserId VALUES" +
                "(@username, @password, @mail, @role, @name, @birthDate, @cnp)";

                using SqlCommand userCommand = new SqlCommand(insertUserQuery, connection, transaction);
                userCommand.Parameters.AddWithValue("@username", model.Username);
                userCommand.Parameters.AddWithValue("@password", model.Password);
                userCommand.Parameters.AddWithValue("@mail", model.Mail);
                userCommand.Parameters.AddWithValue("@name", model.Name);
                userCommand.Parameters.AddWithValue("@role", "Patient");
                userCommand.Parameters.AddWithValue("@birthDate", model.BirthDate);
                userCommand.Parameters.AddWithValue("@cnp", model.Cnp);

                object? userIdObj = await userCommand.ExecuteScalarAsync().ConfigureAwait(false);
                if (userIdObj == null)
                    throw new Exception("User insertion failed.");

                int userId = Convert.ToInt32(userIdObj);

                string insertPatientQuery = @"
                INSERT INTO Patients (UserId, BloodType, EmergencyContact, Weight, Height)
                VALUES (@userId, @bloodType, @emergencyContact, @weight, @height);";

                using SqlCommand patientCommand = new SqlCommand(insertPatientQuery, connection, transaction);
                patientCommand.Parameters.AddWithValue("@userId", userId);
                if (bloodType != null)
                    patientCommand.Parameters.AddWithValue("@bloodType", bloodType);
                patientCommand.Parameters.AddWithValue("@emergencyContact", model.EmergencyContact);
                patientCommand.Parameters.AddWithValue("@weight", model.Weight);
                patientCommand.Parameters.AddWithValue("@height", model.Height);

                int patientResult = await patientCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                transaction.Commit();

                return patientResult == 1;
            }

            catch (AuthenticationException err)
            {
                throw new AuthenticationException(err.Message);
            }

            catch (Exception)
            {
                transaction.Rollback();
                throw new AuthenticationException("Database error");
            }
        }

        public async Task<bool> AuthenticationLogService(int userId, ActionType type)
        {
            string query = "INSERT INTO Logs (UserId, ActionType) VALUES (@userId, @type)";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);

                await connection.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@userId", userId);

                switch (type)
                {
                    case ActionType.LOGIN:
                        command.Parameters.AddWithValue("@type", "LOGIN");
                        break;
                    case ActionType.LOGOUT:
                        command.Parameters.AddWithValue("@type", "LOGOUT");
                        break;
                    default:
                        throw new AuthenticationException("Invalid type for Authentication Log");
                }

                int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                connection.Close();

                return rowsAffected == 1;

            }
            catch (SqlException)
            {
                throw new AuthenticationException("Error Action Logger");
            }
        }
    }
}
