using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.DatabaseServices;
using Hospital.Models;

namespace Hospital.Managers
{
    public class LoggerManagerModel
    {
        private readonly LoggerDatabaseService _loggerDatabaseService;
        private readonly List<LogEntryModel> _logsList;

        public LoggerManagerModel()
        {
            _loggerDatabaseService = new LoggerDatabaseService();
            _logsList = new List<LogEntryModel>();
        }

        public List<LogEntryModel> LogsList => _logsList;

        public async Task LoadAllLogs()
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsFromDB());
        }

        public async Task LoadLogsByUserId(int userId)
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsByUserId(userId));
        }

        public async Task LoadLogsByActionType(ActionType actionType)
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsByActionType(actionType));
        }

        public async Task LoadLogsBeforeTimestamp(DateTime timestamp)
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsBeforeTimestamp(timestamp));
        }

        public async Task LoadLogsWithParameters(int? userId, ActionType actionType, DateTime timestamp)
        {
            _logsList.Clear();
            if(userId != null)
                _logsList.AddRange(await _loggerDatabaseService.GetLogsWithParameters((int)userId, actionType, timestamp));
            else
                _logsList.AddRange(await _loggerDatabaseService.GetLogsWithParametersWithoutUserId(actionType, timestamp));
        }
    }
}
