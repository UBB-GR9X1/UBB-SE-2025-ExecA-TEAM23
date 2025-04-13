// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoggerService.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the ILoggerService interface for accessing system logging functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for logger service operations.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Gets all logs from the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetAllLogs();

        /// <summary>
        /// Gets logs for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose logs to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsByUserId(int userId);

        /// <summary>
        /// Gets logs by action type.
        /// </summary>
        /// <param name="actionType">The action type to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsByActionType(ActionType actionType);

        /// <summary>
        /// Gets logs from before a specific timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsBeforeTimestamp(DateTime timestamp);

        /// <summary>
        /// Gets logs matching specific parameters.
        /// </summary>
        /// <param name="userId">The user ID to filter by.</param>
        /// <param name="actionType">The action type to filter by.</param>
        /// <param name="timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsWithParameters(int userId, ActionType actionType, DateTime timestamp);

        /// <summary>
        /// Gets logs matching specific parameters without filtering by user ID.
        /// </summary>
        /// <param name="actionType">The action type to filter by.</param>
        /// <param name="timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> GetLogsWithParametersWithoutUserId(ActionType actionType, DateTime timestamp);

        /// <summary>
        /// Records a new action in the log.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the action.</param>
        /// <param name="actionType">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> LogAction(int userId, ActionType actionType);
    }
}
