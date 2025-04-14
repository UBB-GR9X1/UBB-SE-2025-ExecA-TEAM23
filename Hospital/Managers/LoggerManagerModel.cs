// <copyright file="LoggerManagerModel.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.DatabaseServices;
    using Hospital.Models;

    /// <summary>
    /// Model for handling system logging operations.
    /// </summary>
    public class LoggerManagerModel : ILoggerManagerModel
    {
        private readonly ILoggerDatabaseService loggerDatabaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerManagerModel"/> class.
        /// </summary>
        /// <param name="loggerDatabaseService">The logger service interface.</param>
        public LoggerManagerModel(ILoggerDatabaseService loggerDatabaseService)
        {
            this.loggerDatabaseService = loggerDatabaseService ?? throw new ArgumentNullException(nameof(loggerDatabaseService));
        }

        /// <summary>
        /// Retrieves all log entries from the system.
        /// </summary>
        /// <returns>A list of all log entries.</returns>
        public async Task<List<LogEntryModel>> GetAllLogs()
        {
            return await this.loggerDatabaseService.GetAllLogs();
        }

        /// <summary>
        /// Retrieves log entries for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of log entries for the specified user.</returns>
        /// <exception cref="ArgumentException">Thrown when the userId is invalid.</exception>
        public async Task<List<LogEntryModel>> GetLogsByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            }

            return await this.loggerDatabaseService.GetLogsByUserId(userId);
        }

        /// <summary>
        /// Retrieves log entries for a specific action type.
        /// </summary>
        /// <param name="actionType">The type of action to filter by.</param>
        /// <returns>A list of log entries for the specified action type.</returns>
        /// <exception cref="ArgumentNullException">This exception won't occur since ActionType is an enum, but kept for interface compliance.</exception>
        public async Task<List<LogEntryModel>> GetLogsByActionType(ActionType actionType)
        {
            // ActionType is an enum, so it can't be null, but we're keeping this check
            // for interface compliance and future-proofing
            // The result of the expression is always 'false'
            if (actionType == null)
            {
                throw new ArgumentNullException(nameof(actionType), "Action type cannot be null.");
            }

            return await this.loggerDatabaseService.GetLogsByActionType(actionType);
        }

        /// <summary>
        /// Retrieves log entries before a specific timestamp.
        /// </summary>
        /// <param name="timestamp">The cutoff timestamp.</param>
        /// <returns>A list of log entries before the specified timestamp.</returns>
        /// <exception cref="ArgumentException">Thrown when the timestamp is default/uninitialized.</exception>
        public async Task<List<LogEntryModel>> GetLogsBeforeTimestamp(DateTime timestamp)
        {
            if (timestamp == default)
            {
                throw new ArgumentException("Timestamp cannot be default.", nameof(timestamp));
            }

            return await this.loggerDatabaseService.GetLogsBeforeTimestamp(timestamp);
        }

        /// <summary>
        /// Retrieves log entries filtered by multiple parameters.
        /// </summary>
        /// <param name="userId">The ID of the user, or null for all users.</param>
        /// <param name="actionType">The type of action.</param>
        /// <param name="timestamp">The cutoff timestamp.</param>
        /// <returns>A list of log entries matching the specified filters.</returns>
        public async Task<List<LogEntryModel>> GetLogsWithParameters(int? userId, ActionType actionType, DateTime timestamp)
        {
            if (userId != null)
            {
                return await this.loggerDatabaseService.GetLogsWithParameters(userId.Value, actionType, timestamp);
            }
            else
            {
                return await this.loggerDatabaseService.GetLogsWithParametersWithoutUserId(actionType, timestamp);
            }
        }

        /// <summary>
        /// Logs a user action in the system.
        /// </summary>
        /// <param name="userId">The ID of the user performing the action.</param>
        /// <param name="actionType">The type of action being performed.</param>
        /// <returns>True if the action was logged successfully, otherwise false.</returns>
        /// <exception cref="ArgumentException">Thrown when the userId is invalid.</exception>
        /// <exception cref="ArgumentNullException">This exception won't occur since ActionType is an enum, but kept for interface compliance.</exception>
        public async Task<bool> LogAction(int userId, ActionType actionType)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            }

            // ActionType is an enum, so it can't be null, but we're keeping this check
            // for interface compliance and future-proofing
            if (actionType == null)
            {
                throw new ArgumentNullException(nameof(actionType), "Action type cannot be null.");
            }

            return await this.loggerDatabaseService.LogAction(userId, actionType);
        }
    }
}
