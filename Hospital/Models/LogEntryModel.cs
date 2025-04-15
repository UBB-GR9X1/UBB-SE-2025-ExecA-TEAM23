namespace Hospital.Models
{
    using System;

    /// <summary>
    /// Represents the types of actions that can be logged in the system.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// User login action.
        /// </summary>
        LOGIN,

        /// <summary>
        /// User logout action.
        /// </summary>
        LOGOUT,

        /// <summary>
        /// Profile update action.
        /// </summary>
        UPDATE_PROFILE,

        /// <summary>
        /// Password change action.
        /// </summary>
        CHANGE_PASSWORD,

        /// <summary>
        /// Account deletion action.
        /// </summary>
        DELETE_ACCOUNT,

        /// <summary>
        /// Account creation action.
        /// </summary>
        CREATE_ACCOUNT,
    }

    /// <summary>
    /// Represents a log entry in the system's activity log.
    /// </summary>
    public class LogEntryModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryModel"/> class.
        /// </summary>
        /// <param name="logId">The unique identifier for the log entry.</param>
        /// <param name="userId">The user ID associated with the action.</param>
        /// <param name="action">The type of action performed.</param>
        /// <param name="timestamp">The date and time when the action occurred.</param>
        public LogEntryModel(int logId, int userId, ActionType action, DateTime timestamp)
        {
            this.LogId = logId;
            this.UserId = userId;
            this.ActionType = action;
            this.Timestamp = timestamp;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the log entry.
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who performed the action.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the type of action performed.
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the action was performed.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Returns a string representation of the log entry.
        /// </summary>
        /// <returns>A string containing the log entry details.</returns>
        public override string ToString()
        {
            return $"LogId: {this.LogId}, UserId: {this.UserId}, Action: {this.ActionType}, Timestamp: {this.Timestamp}";
        }
    }
}
