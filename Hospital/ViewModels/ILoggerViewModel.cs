// <copyright file="ILoggerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Hospital.Models;

    /// <summary>
    /// Interface for LoggerViewModel:
    /// The view model for managing and displaying system logs.
    /// </summary>
    public interface ILoggerViewModel
    {
        /// <summary>
        /// Gets the collection of log entries to display.
        /// </summary>
        ObservableCollection<LogEntryModel> Logs { get; }

        /// <summary>
        /// Gets the command to load all logs.
        /// </summary>
        ICommand LoadAllLogsCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by user ID.
        /// </summary>
        ICommand FilterLogsByUserIdCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by timestamp.
        /// </summary>
        ICommand FilterLogsByTimestampCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by action type.
        /// </summary>
        ICommand FilterLogsByActionTypeCommand { get; }

        /// <summary>
        /// Gets the command to apply all filters simultaneously.
        /// </summary>
        ICommand ApplyAllFiltersCommand { get; }

        /// <summary>
        /// Gets the list of available action types for filtering.
        /// </summary>
        List<ActionType> ActionTypes { get; }

        /// <summary>
        /// Gets or sets the user ID input for filtering.
        /// </summary>
        string UserIdInput { get; set; }

        /// <summary>
        /// Gets or sets the selected action type for filtering.
        /// </summary>
        ActionType SelectedActionType { get; set; }

        /// <summary>
        /// Gets or sets the selected timestamp for filtering.
        /// </summary>
        DateTime SelectedTimestamp { get; set; }
    }
}
