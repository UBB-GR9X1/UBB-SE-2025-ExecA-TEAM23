using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Configs;
using Hospital.Models;
using Microsoft.Data.SqlClient;

namespace Hospital.DatabaseServices
{
    public class LoggerDatabaseService
    {
        private readonly Config _config;

        public LoggerDatabaseService()
        {
            _config = Config.GetInstance();
        }

        public async Task<List<LogEntryModel>> GetLogsFromDB()
        {
            const string queryGetLogs = "SELECT * FROM Logs ORDER BY Timestamp DESC;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(queryGetLogs, connection);

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

                List<LogEntryModel> logs = new List<LogEntryModel>();

                while(await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    ActionType action = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(2));
                    DateTime timestamp = reader.GetDateTime(3);
                    logs.Add(new LogEntryModel(logId, userId, action, timestamp));
                }
                return logs;

            }
            catch (SqlException e) 
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
        }

        public async Task<List<LogEntryModel>> GetLogsByUserId(int user_id) 
        {
            const string queryGetLogsByUserId = "SELECT * FROM Logs WHERE UserId = @UserId;";
            
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(queryGetLogsByUserId, connection);

                selectCommand.Parameters.AddWithValue("@UserId", user_id);

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

                List<LogEntryModel> logs = new List<LogEntryModel>();

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    ActionType action = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(2));
                    DateTime timestamp = reader.GetDateTime(3);
                    logs.Add(new LogEntryModel(logId, userId, action, timestamp));
                }
                return logs;

            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
        }

        public async Task<List<LogEntryModel>> GetLogsBeforeTimestamp(DateTime beforeTimeStamp)
        {
            const string queryGetLogsBeforeTimestamp = "SELECT * FROM Logs WHERE Timestamp < @BeforeTimestamp;";
          
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(queryGetLogsBeforeTimestamp, connection);

                selectCommand.Parameters.AddWithValue("@BeforeTimestamp", beforeTimeStamp);

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

                List<LogEntryModel> logs = new List<LogEntryModel>();

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    ActionType action = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(2));
                    DateTime timestamp = reader.GetDateTime(3);
                    logs.Add(new LogEntryModel(logId, userId, action, timestamp));
                }
                return logs;

            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
        }

        public async Task<List<LogEntryModel>> GetLogsByActionType(ActionType actionType)
        {
            const string queryGetLogsByActionType = "SELECT * FROM Logs \r\nWHERE ActionType = @ActionType;";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(queryGetLogsByActionType, connection);

                selectCommand.Parameters.AddWithValue("@ActionType", actionType.ToString());

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

                List<LogEntryModel> logs = new List<LogEntryModel>();

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    ActionType action = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(2));
                    DateTime timestamp = reader.GetDateTime(3);
                    logs.Add(new LogEntryModel(logId, userId, action, timestamp));
                }
                return logs;

            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
        }

        public async Task<List<LogEntryModel>> GetLogsWithParametersWithoutUserId(ActionType actionType, DateTime beforeTimeStamp)
        {
            const string queryGetLogsByParameters = "SELECT * FROM Logs WHERE ActionType = @ActionType AND Timestamp < @BeforeTimestamp;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand selectCommand = new SqlCommand(queryGetLogsByParameters, connection);
                selectCommand.Parameters.AddWithValue("@ActionType", actionType.ToString());
                selectCommand.Parameters.AddWithValue("@BeforeTimestamp", beforeTimeStamp);
                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                List<LogEntryModel> logs = new List<LogEntryModel>();
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(0);
                    int userIdd = reader.GetInt32(1);
                    ActionType action = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(2));
                    DateTime timestamp = reader.GetDateTime(3);
                    logs.Add(new LogEntryModel(logId, userIdd, action, timestamp));
                }
                return logs;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
        }

        public async Task<List<LogEntryModel>> GetLogsWithParameters(int userId, ActionType actionType, DateTime beforeTimeStamp)
        {
            const string queryGetLogsByParameters = "SELECT * FROM Logs WHERE UserId = @UserId AND ActionType = @ActionType AND Timestamp < @BeforeTimestamp;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand selectCommand = new SqlCommand(queryGetLogsByParameters, connection);
                selectCommand.Parameters.AddWithValue("@UserId", userId);
                selectCommand.Parameters.AddWithValue("@ActionType", actionType.ToString());
                selectCommand.Parameters.AddWithValue("@BeforeTimestamp", beforeTimeStamp);
                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                List<LogEntryModel> logs = new List<LogEntryModel>();
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(0);
                    int userIdd = reader.GetInt32(1);
                    ActionType action = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(2));
                    DateTime timestamp = reader.GetDateTime(3);
                    logs.Add(new LogEntryModel(logId, userIdd, action, timestamp));
                }
                return logs;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<LogEntryModel>();
            }
        }
    }
}
  