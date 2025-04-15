using Hospital.Configs;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Hospital.Repositories;

namespace Hospital.Tests.IntegrationTest
{
    [TestClass]
    public class LoggerDatabaseServiceIntegrationTests
    {
        private Mock<IConfigProvider> _mockConfigProvider;
        private LoggerRepository _loggerRepository;
        private string _testConnectionString = Config.databaseConnection;

        [TestInitialize]
        public void Setup()
        {
            _mockConfigProvider = new Mock<IConfigProvider>();
            _mockConfigProvider.Setup(config => config.GetDatabaseConnection())
                .Returns(_testConnectionString);

            _loggerRepository = new LoggerRepository(_mockConfigProvider.Object);

            // Set up test database
            try
            {
                SetupTestDatabase().Wait();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Database setup error: {exception.Message}");
                throw;
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up test data
            try
            {
                CleanupTestDatabase().Wait();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Database cleanup error: {exception.Message}");
            }
        }

        [TestMethod]
        public async Task GetLogs_GetAllLogs_ReturnsAllLogs()
        {
            try
            {
                // Arrange
                await ClearAllLogs();
                await InsertTestLogs();

                // Act
                var logs = await _loggerRepository.GetAllLogs();

                // Assert
                Assert.IsNotNull(logs, "Logs should not be null");
                Assert.IsTrue(logs.Count > 0, "Logs should contain entries");
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Test exception: {exception.Message}");
                throw;
            }
        }

        [TestMethod]
        public async Task GetLogsByUser_GetLogsByUserId_ReturnsUserLogs()
        {
            try
            {
                // Arrange
                await ClearAllLogs();
                const int testUserId = 99;
                await InsertLogForUser(testUserId, ActionType.LOGIN);

                // Act
                var logs = await _loggerRepository.GetLogsByUserId(testUserId);

                // Assert
                Assert.IsNotNull(logs, "Logs should not be null");
                Assert.IsTrue(logs.Count > 0, "Logs should contain entries for the specified user");
                foreach (var log in logs)
                {
                    Assert.AreEqual(testUserId, log.UserId, "Log should belong to the specified user");
                }

                Debug.WriteLine($"GetLogsByUserId returned {logs.Count} logs for user {testUserId}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Test exception: {exception.Message}");
                throw;
            }
        }

        [TestMethod]
        public async Task GetLogsByAction_GetLogsByActionType_ReturnsFilteredLogs()
        {
            try
            {
                // Arrange
                await ClearAllLogs();
                await InsertLogForUser(1, ActionType.LOGIN);
                await InsertLogForUser(2, ActionType.LOGOUT);

                // Act
                var logs = await _loggerRepository.GetLogsByActionType(ActionType.LOGIN);

                // Assert
                Assert.IsNotNull(logs, "Logs should not be null");
                Assert.IsTrue(logs.Count > 0, "Logs collection should contain at least one item");

                foreach (var log in logs)
                {
                    Debug.WriteLine($"Log action type: {log.ActionType}");
                    Assert.AreEqual(ActionType.LOGIN, log.ActionType, "Each log should have LOGIN action type");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Test exception: {exception.Message}");
                throw;
            }
        }

        [TestMethod]
        public async Task GetLogsBeforeTimestamp_ReturnsFilteredLogs()
        {
            try
            {
                // Arrange
                await ClearAllLogs();
                var now = DateTime.Now;
                var olderTime = now.AddMinutes(-10);
                var newerTime = now.AddMinutes(-5);
                var filterTime = now.AddMinutes(-7);

                await InsertLogForUserWithTimestamp(1, ActionType.LOGIN, olderTime);
                await InsertLogForUserWithTimestamp(2, ActionType.LOGOUT, newerTime);

                // Act
                var logs = await _loggerRepository.GetLogsBeforeTimestamp(filterTime);

                // Assert
                Assert.IsNotNull(logs, "Logs should not be null");
                Assert.IsTrue(logs.Count > 0, "Logs should contain at least one entry");

                foreach (var log in logs)
                {
                    Debug.WriteLine($"Log timestamp: {log.Timestamp}, Filter time: {filterTime}");
                    Assert.IsTrue(log.Timestamp < filterTime,
                        $"Log timestamp {log.Timestamp} should be before filter time {filterTime}");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Test exception: {exception.Message}");
                throw;
            }
        }

        [TestMethod]
        public async Task LogAction_ChecksLogAction_InsertsLog()
        {
            try
            {
                // Arrange
                await ClearAllLogs();
                const int testUserId = 100;
                var actionType = ActionType.CREATE_ACCOUNT;

                // Ensure the test user exists in the database
                await EnsureUserExists(testUserId);

                // Act
                bool result = await _loggerRepository.LogAction(testUserId, actionType);

                // Verify
                var logs = await _loggerRepository.GetLogsByUserId(testUserId);

                // Assert
                Assert.IsTrue(result, "LogAction should return true");
                Assert.IsNotNull(logs, "Retrieved logs should not be null");
                Assert.IsTrue(logs.Count > 0, "There should be logs for the test user");

                var foundLog = false;
                foreach (var log in logs)
                {
                    Debug.WriteLine($"Found log: UserId={log.UserId}, ActionType={log.ActionType}");
                    if (log.UserId == testUserId && log.ActionType == actionType)
                    {
                        foundLog = true;
                        break;
                    }
                }
                Assert.IsTrue(foundLog, "Should find a log with the correct user ID and action type");
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Test exception: {exception.Message}");
                throw;
            }
        }

        private async Task ClearAllLogs()
        {
            using (var connection = new SqlConnection(_testConnectionString))
            {
                await connection.OpenAsync();
                string clearLogsQuery = "DELETE FROM Logs";
                using (var command = new SqlCommand(clearLogsQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task EnsureUserExists(int userId)
        {
            using (var connection = new SqlConnection(_testConnectionString))
            {
                await connection.OpenAsync();

                // Check if the user already exists
                const string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE UserId = @userId";
                bool userExists = false;

                using (var command = new SqlCommand(checkUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    var result = await command.ExecuteScalarAsync();
                    userExists = Convert.ToInt32(result) > 0;
                }

                if (!userExists)
                {
                    // Insert the test user with all required fields
                    const string insertUserQuery = @"
                        SET IDENTITY_INSERT Users ON;
                        INSERT INTO Users (UserId, Username, Password, Mail, Role, Name, BirthDate, Cnp)
                        VALUES (@userId, 'testuser' + CAST(@userId AS NVARCHAR), 'password', 
                               'test' + CAST(@userId AS NVARCHAR) + '@example.com', 'User',
                               'Test User ' + CAST(@userId AS NVARCHAR), @birthDate, @cnp);
                        SET IDENTITY_INSERT Users OFF;";

                    using (var command = new SqlCommand(insertUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@birthDate", DateTime.Now.AddYears(-25));
                        command.Parameters.AddWithValue("@cnp", userId.ToString().PadLeft(10, '0') + "123"); // 13 digits
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private async Task SetupTestDatabase()
        {
            using (var connection = new SqlConnection(_testConnectionString))
            {
                await connection.OpenAsync();

                // Check if the Users table exists
                const string checkUsersTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    BEGIN
                        CREATE TABLE Users (
                            UserId INT IDENTITY(1,1) PRIMARY KEY,
                            Username NVARCHAR(50) NOT NULL,
                            Password NVARCHAR(100) NOT NULL,
                            Mail NVARCHAR(100) NOT NULL,
                            Role NVARCHAR(20) NOT NULL,
                            Name NVARCHAR(100) NOT NULL,
                            BirthDate DATE NOT NULL,
                            Cnp NVARCHAR(13) NOT NULL
                        );
                    END";

                using (var command = new SqlCommand(checkUsersTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // Check if the Logs table exists
                const string checkLogsTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Logs')
                    BEGIN
                        CREATE TABLE Logs (
                            LogId INT IDENTITY(1,1) PRIMARY KEY,
                            UserId INT NOT NULL,
                            ActionType NVARCHAR(50) NOT NULL,
                            Timestamp DATETIME NOT NULL
                        );
                    END";

                using (var command = new SqlCommand(checkLogsTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // Check if foreign key exists and add it if it doesn't
                const string checkForeignKeyQuery = @"
                    IF NOT EXISTS (
                        SELECT * FROM sys.foreign_keys 
                        WHERE name = 'FK_Logs_Users' AND parent_object_id = OBJECT_ID('Logs')
                    )
                    BEGIN
                        ALTER TABLE Logs ADD CONSTRAINT FK_Logs_Users 
                        FOREIGN KEY (UserId) REFERENCES Users(UserId);
                    END";

                using (var command = new SqlCommand(checkForeignKeyQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // Insert test users for foreign key relationship
                const string insertTestUsers = @"
                    IF NOT EXISTS (SELECT * FROM Users WHERE UserId IN (1, 2, 99, 100))
                    BEGIN
                        SET IDENTITY_INSERT Users ON;
                        
                        INSERT INTO Users (UserId, Username, Password, Mail, Role, Name, BirthDate, Cnp)
                        VALUES 
                        (1, 'testuser1', 'password', 'test1@example.com', 'User', 'Test User 1', GETDATE(), '1000000000123'),
                        (2, 'testuser2', 'password', 'test2@example.com', 'User', 'Test User 2', GETDATE(), '2000000000123'),
                        (99, 'testuser99', 'password', 'test99@example.com', 'User', 'Test User 99', GETDATE(), '9900000000123'),
                        (100, 'testuser100', 'password', 'test100@example.com', 'User', 'Test User 100', GETDATE(), '1000000000123');
                        
                        SET IDENTITY_INSERT Users OFF;
                    END";

                using (var command = new SqlCommand(insertTestUsers, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task CleanupTestDatabase()
        {
            using (var connection = new SqlConnection(_testConnectionString))
            {
                await connection.OpenAsync();

                const string cleanupQuery = "DELETE FROM Logs WHERE UserId IN (1, 2, 99, 100)";

                using (var command = new SqlCommand(cleanupQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // You might want to keep this commented out to preserve test users
                // const string cleanupUsersQuery = "DELETE FROM Users WHERE UserId IN (1, 2, 99, 100)";
                // using (var command = new SqlCommand(cleanupUsersQuery, connection))
                // {
                //     await command.ExecuteNonQueryAsync();
                // }
            }
        }

        private async Task InsertTestLogs()
        {
            await InsertLogForUser(1, ActionType.LOGIN);
            await InsertLogForUser(1, ActionType.LOGOUT);
            await InsertLogForUser(2, ActionType.UPDATE_PROFILE);
        }

        private async Task InsertLogForUser(int userId, ActionType actionType)
        {
            await InsertLogForUserWithTimestamp(userId, actionType, DateTime.Now);
        }

        private async Task InsertLogForUserWithTimestamp(int userId, ActionType actionType, DateTime timestamp)
        {
            try
            {
                using (var connection = new SqlConnection(_testConnectionString))
                {
                    await connection.OpenAsync();

                    // First ensure user exists
                    await EnsureUserExists(userId);

                    // Now insert the log
                    const string insertQuery = @"
                        INSERT INTO Logs (UserId, ActionType, Timestamp) 
                        VALUES (@userId, @actionType, @timestamp)";

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@actionType", actionType.ToString());
                        command.Parameters.AddWithValue("@timestamp", timestamp);
                        await command.ExecuteNonQueryAsync();
                    }

                    Debug.WriteLine($"Inserted log for user {userId}, action {actionType}, time {timestamp}");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error inserting log: {exception.Message}");
                throw;
            }
        }
    }
}