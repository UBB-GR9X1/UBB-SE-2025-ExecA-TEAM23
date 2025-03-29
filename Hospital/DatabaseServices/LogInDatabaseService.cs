using Hospital.Configs;
using Hospital.Exceptions;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

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

            try
            {
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
                    return new UserAuthModel(userId, userName, password, mail);
                }
                throw new AuthenticationException("No user found with given username");
            }
            catch (AuthenticationException e)
            {
                throw e;
            }
            catch (SqlException e)
            {
                throw new Exception($"SQL Exception: {e.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Error loading Users: {e.Message}");
            }
        }
    }
}
    