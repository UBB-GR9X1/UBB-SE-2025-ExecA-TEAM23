using System;

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

    public class LogEntryModel
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime Timestamp { get; set; }

        public LogEntryModel(int logId, int userId, ActionType action, DateTime timestamp)
        {
            LogId = logId;
            UserId = userId;
            ActionType = action;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"LogId: {LogId}, UserId: {UserId}, Action: {ActionType}, Timestamp: {Timestamp}";
        }
    }
}
