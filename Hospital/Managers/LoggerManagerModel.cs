using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async void LoadAllLogs()
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsFromDB());
        }

        public async void LoadLogsByUserId(int userId)
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsByUserId(userId));
        }

        public async void LoadLogsByActionType(ActionType actionType)
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsByActionType(actionType));
        }

        public async void LoadLogsBeforeTimestamp(DateTime timestamp)
        {
            _logsList.Clear();
            _logsList.AddRange(await _loggerDatabaseService.GetLogsBeforeTimestamp(timestamp));
        }
    }
}
