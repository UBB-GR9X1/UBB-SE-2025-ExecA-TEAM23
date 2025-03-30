using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Hospital.Managers;
using Hospital.Models;

namespace Hospital.ViewModels
{
    public class LoggerViewModel : BaseViewModel
    {
        private readonly LoggerManagerModel _loggerManager;

        public ObservableCollection<LogEntryModel> Logs { get; private set; }

        // Commands for filtering logs
        public ICommand LoadAllLogsCommand { get; }
        public ICommand LoadLogsByUserIdCommand { get; }
        public ICommand LoadLogsBeforeTimestampCommand { get; }
        public ICommand LoadLogsByActionTypeCommand { get; }
        public ICommand LoadLogsWithParametersCommand { get; }

        private string _userIdInput;

        private ActionType _selectedActionType;
        public List<ActionType> ActionTypes { get; } = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

        private DateTime _selectedTimestamp = DateTime.Now;

        public string UserIdInput
        {
            get => _userIdInput;
            set { _userIdInput = value; OnPropertyChanged(); }
        }

        public ActionType SelectedActionType
        {
            get => _selectedActionType;
            set
            {
                if (_selectedActionType != value)
                {
                    _selectedActionType = value;
                    Debug.WriteLine($"SelectedActionType updated: {_selectedActionType}");
                    OnPropertyChanged();
                }
            }
        }
        public DateTime SelectedTimestamp
        {
            get => _selectedTimestamp;
            set
            {
                if (_selectedTimestamp != value)
                {
                    _selectedTimestamp = value;
                    Debug.WriteLine($"SelectedTimestamp updated: {_selectedTimestamp}");
                    OnPropertyChanged();
                }
            }
        }


        public LoggerViewModel(LoggerManagerModel loggerManager)
        {
            _loggerManager = loggerManager;
            Logs = new ObservableCollection<LogEntryModel>();

            LoadAllLogsCommand = new RelayCommand(async () => await LoadAllLogsAsync());
            LoadLogsByUserIdCommand = new RelayCommand(async () => await LoadLogsByUserIdAsync());
            LoadLogsBeforeTimestampCommand = new RelayCommand(async () => await LoadLogsBeforeTimestampAsync());
            LoadLogsByActionTypeCommand = new RelayCommand(async () => await LoadLogsByActionTypeAsync());
            LoadLogsWithParametersCommand = new RelayCommand(async () => await LoadLogsWithParametersAsync());

        }

        private async Task LoadAllLogsAsync()
        {
            await _loggerManager.LoadAllLogs();
            UpdateLogs();
        }

        private async Task LoadLogsByUserIdAsync()
        {
            if (string.IsNullOrWhiteSpace(UserIdInput))
            {
                Debug.WriteLine("No UserId provided, loading all logs...");
                await _loggerManager.LoadAllLogs();
                UpdateLogs();
                return;
            }

            if (int.TryParse(UserIdInput, out int userId))
            {
                await _loggerManager.LoadLogsByUserId(userId);
                UpdateLogs();
            }
        }

        private async Task LoadLogsBeforeTimestampAsync()
        {
            await _loggerManager.LoadLogsBeforeTimestamp(SelectedTimestamp);
            UpdateLogs();
        }

        private async Task LoadLogsByActionTypeAsync()
        {
            Debug.WriteLine(SelectedActionType);
            await _loggerManager.LoadLogsByActionType(SelectedActionType);
            UpdateLogs();
        }

        private async Task LoadLogsWithParametersAsync()
        {
            int? userId = null;

            // Try to parse UserIdInput; if successful, assign the value, otherwise keep it null
            if (int.TryParse(UserIdInput, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            await _loggerManager.LoadLogsWithParameters(userId, SelectedActionType, SelectedTimestamp);
            UpdateLogs();
        }

        private void UpdateLogs()
        {
            Debug.WriteLine("Updating logs...");
            Logs.Clear();
            foreach (var log in _loggerManager.LogsList)
            {
                Logs.Add(log);
            }
        }
    }
}
