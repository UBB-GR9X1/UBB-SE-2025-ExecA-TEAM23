// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerViewModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the LoggerViewModel for handling system logging operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Hospital.Interfaces;
    using Hospital.Models;

    /// <summary>
    /// View model for managing and displaying system logs.
    /// </summary>
    public class LoggerViewModel : BaseViewModel
    {
        private readonly ILoggerManagerModel _loggerManager;
        private string _userIdInput = string.Empty;
        private ActionType _selectedActionType;
        private DateTime _selectedTimestamp = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerViewModel"/> class.
        /// </summary>
        /// <param name="loggerManager">The logger manager model interface.</param>
        public LoggerViewModel(ILoggerManagerModel loggerManager)
        {
            this._loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
            this.Logs = new ObservableCollection<LogEntryModel>();
            this.ActionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

            // Initialize commands
            this.LoadAllLogsCommand = new RelayCommand(async () => await this.ExecuteLoadAllLogsAsync());
            this.FilterLogsByUserIdCommand = new RelayCommand(async () => await this.ExecuteFilterLogsByUserIdAsync());
            this.FilterLogsByTimestampCommand = new RelayCommand(async () => await this.ExecuteFilterLogsByTimestampAsync());
            this.FilterLogsByActionTypeCommand = new RelayCommand(async () => await this.ExecuteFilterLogsByActionTypeAsync());
            this.ApplyAllFiltersCommand = new RelayCommand(async () => await this.ExecuteApplyAllFiltersAsync());
        }

        /// <summary>
        /// Gets the collection of log entries to display.
        /// </summary>
        public ObservableCollection<LogEntryModel> Logs { get; private set; }

        /// <summary>
        /// Gets the command to load all logs.
        /// </summary>
        public ICommand LoadAllLogsCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by user ID.
        /// </summary>
        public ICommand FilterLogsByUserIdCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by timestamp.
        /// </summary>
        public ICommand FilterLogsByTimestampCommand { get; }

        /// <summary>
        /// Gets the command to filter logs by action type.
        /// </summary>
        public ICommand FilterLogsByActionTypeCommand { get; }

        /// <summary>
        /// Gets the command to apply all filters simultaneously.
        /// </summary>
        public ICommand ApplyAllFiltersCommand { get; }

        /// <summary>
        /// Gets the list of available action types for filtering.
        /// </summary>
        public List<ActionType> ActionTypes { get; }

        /// <summary>
        /// Gets or sets the user ID input for filtering.
        /// </summary>
        public string UserIdInput
        {
            get => this._userIdInput;
            set
            {
                this._userIdInput = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected action type for filtering.
        /// </summary>
        public ActionType SelectedActionType
        {
            get => this._selectedActionType;
            set
            {
                if (this._selectedActionType != value)
                {
                    this._selectedActionType = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected timestamp for filtering.
        /// </summary>
        public DateTime SelectedTimestamp
        {
            get => this._selectedTimestamp;
            set
            {
                if (this._selectedTimestamp != value)
                {
                    this._selectedTimestamp = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads all available logs from the data source.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ExecuteLoadAllLogsAsync()
        {
            var logEntries = await this._loggerManager.GetAllLogs();
            this.UpdateLogsCollection(logEntries);
        }

        /// <summary>
        /// Filters logs by the provided user ID.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ExecuteFilterLogsByUserIdAsync()
        {
            if (string.IsNullOrWhiteSpace(this.UserIdInput))
            {
                var allLogs = await this._loggerManager.GetAllLogs();
                this.UpdateLogsCollection(allLogs);
                return;
            }

            if (int.TryParse(this.UserIdInput, out int userId))
            {
                try
                {
                    var filteredLogs = await this._loggerManager.GetLogsByUserId(userId);
                    this.UpdateLogsCollection(filteredLogs);
                }
                catch (ArgumentException)
                {
                    // Handle invalid user ID gracefully - show all logs instead
                    var allLogs = await this._loggerManager.GetAllLogs();
                    this.UpdateLogsCollection(allLogs);
                }
            }
        }

        /// <summary>
        /// Filters logs by the selected timestamp.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ExecuteFilterLogsByTimestampAsync()
        {
            try
            {
                var filteredLogs = await this._loggerManager.GetLogsBeforeTimestamp(this.SelectedTimestamp);
                this.UpdateLogsCollection(filteredLogs);
            }
            catch (ArgumentException)
            {
                // Handle invalid timestamp gracefully
                var allLogs = await this._loggerManager.GetAllLogs();
                this.UpdateLogsCollection(allLogs);
            }
        }

        /// <summary>
        /// Filters logs by the selected action type.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ExecuteFilterLogsByActionTypeAsync()
        {
            try
            {
                var filteredLogs = await this._loggerManager.GetLogsByActionType(this.SelectedActionType);
                this.UpdateLogsCollection(filteredLogs);
            }
            catch (ArgumentNullException)
            {
                // Handle null action type gracefully
                var allLogs = await this._loggerManager.GetAllLogs();
                this.UpdateLogsCollection(allLogs);
            }
        }

        /// <summary>
        /// Applies all selected filters simultaneously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ExecuteApplyAllFiltersAsync()
        {
            int? userId = null;

            if (int.TryParse(this.UserIdInput, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            try
            {
                var filteredLogs = await this._loggerManager.GetLogsWithParameters(userId, this.SelectedActionType, this.SelectedTimestamp);
                this.UpdateLogsCollection(filteredLogs);
            }
            catch (Exception)
            {
                // Handle any exceptions gracefully
                var allLogs = await this._loggerManager.GetAllLogs();
                this.UpdateLogsCollection(allLogs);
            }
        }

        /// <summary>
        /// Updates the logs collection with the provided entries.
        /// </summary>
        /// <param name="logEntries">The log entries to display.</param>
        private void UpdateLogsCollection(IEnumerable<LogEntryModel> logEntries)
        {
            this.Logs.Clear();

            if (logEntries != null)
            {
                foreach (var logEntry in logEntries)
                {
                    this.Logs.Add(logEntry);
                }
            }
        }
    }
}
