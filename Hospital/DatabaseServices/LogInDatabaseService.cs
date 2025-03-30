using Hospital.Configs;
using Hospital.Exceptions;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;

namespace Hospital.DatabaseServices
{
    public class LogInDatabaseService
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
                connection.Close();
                return new UserAuthModel(userId, userName, password, mail);
            }
            connection.Close();
            throw new AuthenticationException("No user found with given username");
        }

        public async Task<bool> CreateAccount(string username, string password, string mail, string name, DateOnly birthDate, string cnp)
        {
            // check if there's already a user with given username, mail, cnp
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username OR Mail = @email OR Cnp = @cnp;";
            using SqlConnection checkConnection = new SqlConnection(_config.DatabaseConnection);
            await checkConnection.OpenAsync().ConfigureAwait(false);
            using SqlCommand checkCommand = new SqlCommand(checkQuery, checkConnection);
            checkCommand.Parameters.AddWithValue("@username", username);
            checkCommand.Parameters.AddWithValue("@email", mail);
            checkCommand.Parameters.AddWithValue("@cnp", cnp);

            object? resultCheck = await checkCommand.ExecuteScalarAsync().ConfigureAwait(false);

            if (resultCheck != null && Convert.ToInt32(resultCheck) > 0)
                throw new AuthenticationException("User already exists!");

            checkConnection.Close();

            // implement actual add

            string query = "INSERT INTO Users(Username, Password, Mail, Name, BirthDate, Cnp) VALUES" +
                "(@username, @password, @mail, @name, @birthDate, @cnp)";

            using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);

            await connection.OpenAsync().ConfigureAwait(false);

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@mail", mail);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@birthDate", birthDate);
            command.Parameters.AddWithValue("@cnp", cnp);

            int result = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

            connection.Close();

            return result == 1;
        }

        public async Task<bool> AuthenticationLogService (int userId, ActionType type)
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
 