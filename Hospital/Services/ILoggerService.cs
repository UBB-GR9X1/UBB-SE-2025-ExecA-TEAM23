// <copyright file="ILoggerService.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for managing system logging operations.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Retrieves all logs from the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetAllLogs();

        /// <summary>
        /// Retrieves logs for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose logs to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsByUserId(int userId);

        /// <summary>
        /// Retrieves logs of a specific action type.
        /// </summary>
        /// <param name="actionType">The type of action to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsByActionType(ActionType actionType);

        /// <summary>
        /// Retrieves logs recorded before a specific timestamp.
        /// </summary>
        /// <param name="timestamp">The cutoff timestamp.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsBeforeTimestamp(DateTime timestamp);

        /// <summary>
        /// Retrieves logs that match multiple filter criteria.
        /// </summary>
        /// <param name="userId">Optional user ID to filter by.</param>
        /// <param name="actionType">The action type to filter by.</param>
        /// <param name="timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsWithParameters(int? userId, ActionType actionType, DateTime timestamp);

        /// <summary>
        /// Records a new action in the log.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the action.</param>
        /// <param name="actionType">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> LogAction(int userId, ActionType actionType);
    }
}
