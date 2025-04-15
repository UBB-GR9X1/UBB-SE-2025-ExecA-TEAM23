// <copyright file="LoggerDatabaseService.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.DatabaseServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Configs;
    using Hospital.Models;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Service for handling database operations related to system logs.
    /// </summary>
    public class LoggerDatabaseService : ILoggerDatabaseService
    {
        private const int LogIdColumnIndex = 0;
        private const int UserIdColumnIndex = 1;
        private const int ActionTypeColumnIndex = 2;
        private const int TimestampColumnIndex = 3;

        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerDatabaseService"/> class.
        /// </summary>
        /// <param name="configProvider">The configuration provider for database connection information.</param>
        /// <exception cref="ArgumentNullException">Thrown when configProvider is null.</exception>
        public LoggerDatabaseService()
        {
            var config = Config.GetInstance();
            connectionString = config.GetDatabaseConnection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerDatabaseService"/> class.
        /// </summary>
        /// <param name="configProvider">The configuration provider for database connection information.</param>
        /// <exception cref="ArgumentNullException">Thrown when configProvider is null.</exception>
        public LoggerDatabaseService(IConfigProvider configProvider)
        {
            if (configProvider == null)
                throw new ArgumentNullException(nameof(configProvider));

            connectionString = configProvider.GetDatabaseConnection();
        }

        /// <summary>
        /// Gets all logs from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> GetAllLogs()
        {
            const string queryGetLogs = "SELECT * FROM Logs ORDER BY Timestamp DESC;";
            return await this.ExecuteLogRetrievalQuery(queryGetLogs, null);
        }

        /// <summary>
        /// Gets logs for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose logs to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> GetLogsByUserId(int userId)
        {
            const string queryGetLogsByUserId = "SELECT * FROM Logs WHERE UserId = @UserId;";
            SqlParameter[] parameters = { new SqlParameter("@UserId", userId) };
            return await this.ExecuteLogRetrievalQuery(queryGetLogsByUserId, parameters);
        }

        /// <summary>
        /// Gets logs from before a specific timestamp.
        /// </summary>
        /// <param name="beforeTimeStamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> GetLogsBeforeTimestamp(DateTime beforeTimeStamp)
        {
            const string queryGetLogsBeforeTimestamp = "SELECT * FROM Logs WHERE Timestamp < @BeforeTimestamp;";
            SqlParameter[] parameters = { new SqlParameter("@BeforeTimestamp", beforeTimeStamp) };
            return await this.ExecuteLogRetrievalQuery(queryGetLogsBeforeTimestamp, parameters);
        }

        /// <summary>
        /// Records a new action in the log.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the action.</param>
        /// <param name="actionType">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        public async Task<bool> LogAction(int userId, ActionType actionType)
        {
            const string queryInsertLog = "INSERT INTO Logs (UserId, ActionType, Timestamp) VALUES (@UserId, @ActionType, @Timestamp);";
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ActionType", actionType.ToString()),
                new SqlParameter("@Timestamp", DateTime.UtcNow),
            };
            return await this.ExecuteLogInsertQuery(queryInsertLog, parameters);
        }

        /// <summary>
        /// Gets logs by action type.
        /// </summary>
        /// <param name="actionType">The action type to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> GetLogsByActionType(ActionType actionType)
        {
            const string queryGetLogsByActionType = "SELECT * FROM Logs WHERE ActionType = @ActionType;";
            SqlParameter[] parameters = { new SqlParameter("@ActionType", actionType.ToString()) };
            return await this.ExecuteLogRetrievalQuery(queryGetLogsByActionType, parameters);
        }

        /// <summary>
        /// Gets logs matching specific parameters without filtering by user ID.
        /// </summary>
        /// <param name="actionType">The action type to filter by.</param>
        /// <param name="beforeTimeStamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> GetLogsWithParametersWithoutUserId(ActionType actionType, DateTime beforeTimeStamp)
        {
            const string queryGetLogsByParameters = "SELECT * FROM Logs WHERE ActionType = @ActionType AND Timestamp < @BeforeTimestamp;";
            SqlParameter[] parameters =
            {
                new SqlParameter("@ActionType", actionType.ToString()),
                new SqlParameter("@BeforeTimestamp", beforeTimeStamp),
            };
            return await this.ExecuteLogRetrievalQuery(queryGetLogsByParameters, parameters);
        }

        /// <summary>
        /// Gets logs matching specific parameters.
        /// </summary>
        /// <param name="userId">The user ID to filter by.</param>
        /// <param name="actionType">The action type to filter by.</param>
        /// <param name="beforeTimeStamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> GetLogsWithParameters(int userId, ActionType actionType, DateTime beforeTimeStamp)
        {
            const string queryGetLogsByParameters = "SELECT * FROM Logs WHERE UserId = @UserId AND ActionType = @ActionType AND Timestamp < @BeforeTimestamp;";
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ActionType", actionType.ToString()),
                new SqlParameter("@BeforeTimestamp", beforeTimeStamp),
            };
            return await this.ExecuteLogRetrievalQuery(queryGetLogsByParameters, parameters);
        }

        /// <summary>
        /// Executes a SQL query to retrieve log entries.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters to use in the query.</param>
        /// <returns>A list of log entries from the query result.</returns>
        private async Task<List<LogEntryModel>> ExecuteLogRetrievalQuery(string query, SqlParameter[]? parameters)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(this.connectionString);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    selectCommand.Parameters.AddRange(parameters);
                }

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                List<LogEntryModel> logEntries = new List<LogEntryModel>();

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int logId = reader.GetInt32(LogIdColumnIndex);
                    int userId = reader.GetInt32(UserIdColumnIndex);
                    ActionType actionType = (ActionType)Enum.Parse(typeof(ActionType), reader.GetString(ActionTypeColumnIndex));
                    DateTime timestamp = reader.GetDateTime(TimestampColumnIndex);
                    logEntries.Add(new LogEntryModel(logId, userId, actionType, timestamp));
                }

                return logEntries;
            }
            catch (SqlException exception)
            {
                Console.WriteLine($"SQL Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"General Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
        }

        /// <summary>
        /// Executes a SQL query to insert a log entry.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters to use in the query.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        private async Task<bool> ExecuteLogInsertQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(this.connectionString);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand insertCommand = new SqlCommand(query, connection);
                insertCommand.Parameters.AddRange(parameters);
                int rowsAffected = await insertCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException exception)
            {
                Console.WriteLine($"SQL Exception: {exception.Message}");
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"General Exception: {exception.Message}");
                return false;
            }
        }
    }
}