using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    // Open for more actions
    public enum ActionType
    {
        LOGIN,
        LOGOUT,
        UPDATE_PROFILE,
        CHANGE_PASSWORD,
        DELETE_ACCOUNT
    }

    public class LogEntry
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public ActionType Action { get; set; }
        public DateTime Timestamp { get; set; }

        public LogEntry(int logId, int userId, ActionType action, DateTime timestamp)
        {
            LogId = logId;
            UserId = userId;
            Action = action;
            Timestamp = timestamp;
        }
    }
}
