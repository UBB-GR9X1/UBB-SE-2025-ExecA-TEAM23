// <copyright file="LoggerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Hospital.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Hospital.Managers;
    using Hospital.Models;

    /// <summary>
    /// View model for managing and displaying system logs.
    /// </summary>
    public class LoggerViewModel : BaseViewModel, ILoggerViewModel
    {
        private readonly ILoggerManagerModel loggerManager;
        private string userIdInput = string.Empty;
        private ActionType selectedActionType;
        private DateTime selectedTimestamp = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerViewModel"/> class.
        /// </summary>
        /// <param name="loggerManager">The logger manager model interface.</param>
        public LoggerViewModel(ILoggerManagerModel loggerManager)
        {
            this.loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
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
            get => this.userIdInput;
            set
            {
                this.userIdInput = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected action type for filtering.
        /// </summary>
        public ActionType SelectedActionType
        {
            get => this.selectedActionType;
            set
            {
                if (this.selectedActionType != value)
                {
                    this.selectedActionType = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected timestamp for filtering.
        /// </summary>
        public DateTime SelectedTimestamp
        {
            get => this.selectedTimestamp;
            set
            {
                if (this.selectedTimestamp != value)
                {
                    this.selectedTimestamp = value;
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
            var logEntries = await this.loggerManager.GetAllLogs();
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
                var allLogs = await this.loggerManager.GetAllLogs();
                this.UpdateLogsCollection(allLogs);
                return;
            }

            if (int.TryParse(this.UserIdInput, out int userId))
            {
                try
                {
                    var filteredLogs = await this.loggerManager.GetLogsByUserId(userId);
                    this.UpdateLogsCollection(filteredLogs);
                }
                catch (ArgumentException)
                {
                    // Handle invalid user ID gracefully - show all logs instead
                    var allLogs = await this.loggerManager.GetAllLogs();
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
                var filteredLogs = await this.loggerManager.GetLogsBeforeTimestamp(this.SelectedTimestamp);
                this.UpdateLogsCollection(filteredLogs);
            }
            catch (ArgumentException)
            {
                // Handle invalid timestamp gracefully
                var allLogs = await this.loggerManager.GetAllLogs();
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
                var filteredLogs = await this.loggerManager.GetLogsByActionType(this.SelectedActionType);
                this.UpdateLogsCollection(filteredLogs);
            }
            catch (ArgumentNullException)
            {
                // Handle null action type gracefully
                var allLogs = await this.loggerManager.GetAllLogs();
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
                var filteredLogs = await this.loggerManager.GetLogsWithParameters(userId, this.SelectedActionType, this.SelectedTimestamp);
                this.UpdateLogsCollection(filteredLogs);
            }
            catch (Exception)
            {
                // Handle any exceptions gracefully
                var allLogs = await this.loggerManager.GetAllLogs();
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
