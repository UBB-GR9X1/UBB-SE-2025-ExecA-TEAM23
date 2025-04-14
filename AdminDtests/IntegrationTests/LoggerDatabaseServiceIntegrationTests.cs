using Hospital.Configs;
using Hospital.DatabaseServices;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Interfaces;

namespace Hospital.Tests.IntegrationTest
{
    [TestClass]
    public class LoggerDatabaseServiceIntegrationTests
    {
        private Mock<IConfigProvider> _mockConfigProvider;
        private LoggerDatabaseService _loggerService;
        private string _testConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HospitalTestDB;Integrated Security=True";

        [TestInitialize]
        public void Setup()
        {
            _mockConfigProvider = new Mock<IConfigProvider>();
            _mockConfigProvider.Setup(config => config.GetDatabaseConnection())
                .Returns(_testConnectionString);
            
            _loggerService = new LoggerDatabaseService(_mockConfigProvider.Object);
            
            // Set up test database
            SetupTestDatabase().Wait();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up test data
            CleanupTestDatabase().Wait();
        }

        [TestMethod]
        public async Task GetAllLogs_ReturnsAllLogs()
        {
            // Arrange
            await InsertTestLogs();

            // Act
            var logs = await _loggerService.GetAllLogs();

            // Assert
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Count > 0);
        }

        [TestMethod]
        public async Task GetLogsByUserId_ValidId_ReturnsUserLogs()
        {
            // Arrange
            const int testUserId = 99;
            await InsertLogForUser(testUserId, ActionType.LOGIN);

            // Act
            var logs = await _loggerService.GetLogsByUserId(testUserId);

            // Assert
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Count > 0);
            foreach (var log in logs)
            {
                Assert.AreEqual(testUserId, log.UserId);
            }
        }

        [TestMethod]
        public async Task GetLogsByActionType_ValidType_ReturnsFilteredLogs()
        {
            // Arrange
            await InsertLogForUser(1, ActionType.LOGIN);
            await InsertLogForUser(2, ActionType.LOGOUT);
            
            // Act
            var logs = await _loggerService.GetLogsByActionType(ActionType.LOGIN);
            
            // Assert
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Count > 0);
            foreach (var log in logs)
            {
                Assert.AreEqual(ActionType.LOGIN, log.ActionType);
            }
        }

        [TestMethod]
        public async Task LogAction_ValidData_InsertsLog()
        {
            // Arrange
            const int testUserId = 100;
            var actionType = ActionType.CREATE_ACCOUNT;

            // Ensure the test user exists in the database
            await InsertTestUser(testUserId);

            // Act
            bool result = await _loggerService.LogAction(testUserId, actionType);

            // Verify
            var logs = await _loggerService.GetLogsByUserId(testUserId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(logs);
            Assert.IsTrue(logs.Count > 0);

            var foundLog = false;
            foreach (var log in logs)
            {
                if (log.UserId == testUserId && log.ActionType == actionType)
                {
                    foundLog = true;
                    break;
                }
            }
            Assert.IsTrue(foundLog);
        }

        // Helper method to create test users
        private async Task InsertTestUser(int userId)
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

                // Create test tables if they don't exist
                const string createUsersTable = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                    CREATE TABLE Users (
                        UserId INT PRIMARY KEY IDENTITY(1,1),
                        Username NVARCHAR(50) NOT NULL,
                        Password NVARCHAR(255) NOT NULL,
                        Mail NVARCHAR(100) NOT NULL,
                        Role NVARCHAR(50) NOT NULL,
                        Name NVARCHAR(100),
                        BirthDate DATETIME,
                        Cnp NVARCHAR(13),
                        Address NVARCHAR(255),
                        PhoneNumber NVARCHAR(20),
                        RegistrationDate DATETIME DEFAULT GETDATE()
                    )";

                // Add the Logs table with a proper foreign key constraint
                const string createLogsTable = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Logs' AND xtype='U')
                    BEGIN
                        CREATE TABLE Logs (
                            LogId INT PRIMARY KEY IDENTITY(1,1),
                            UserId INT NOT NULL,
                            ActionType NVARCHAR(50) NOT NULL,
                            Timestamp DATETIME DEFAULT GETDATE(),
                            CONSTRAINT FK_Logs_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
                        )
                    END";

                using (var command = new SqlCommand(createUsersTable, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command = new SqlCommand(createLogsTable, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // Drop and recreate the foreign key if it exists but is causing issues
                const string alterLogsFKQuery = @"
                    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Logs_Users')
                    BEGIN
                        ALTER TABLE Logs DROP CONSTRAINT FK_Logs_Users;
                        ALTER TABLE Logs ADD CONSTRAINT FK_Logs_Users FOREIGN KEY (UserId) REFERENCES Users(UserId);
                    END";

                using (var command = new SqlCommand(alterLogsFKQuery, connection))
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
            using (var connection = new SqlConnection(_testConnectionString))
            {
                await connection.OpenAsync();

                // First check if the user exists and insert with all required fields
                const string checkUserQuery = @"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @userId) 
            BEGIN 
                SET IDENTITY_INSERT Users ON; 
                INSERT INTO Users (UserId, Username, Password, Mail, Role, Name, BirthDate, Cnp) 
                VALUES (@userId, 'testuser' + CAST(@userId AS NVARCHAR), 'password', 
                       'test' + CAST(@userId AS NVARCHAR) + '@example.com', 'User', 
                       'Test User ' + CAST(@userId AS NVARCHAR), @birthDate, 
                       @cnp); 
                SET IDENTITY_INSERT Users OFF; 
            END";

                using (var checkCommand = new SqlCommand(checkUserQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@userId", userId);
                    checkCommand.Parameters.AddWithValue("@birthDate", DateTime.Now.AddYears(-25)); // Default birth date

                    // Generate a unique CNP for each test user (13 digits as per schema constraint)
                    string cnp = userId.ToString().PadLeft(10, '0') + "123"; // Creates a 13-digit CNP based on userId
                    checkCommand.Parameters.AddWithValue("@cnp", cnp);

                    await checkCommand.ExecuteNonQueryAsync();
                }

                // Now insert the log
                const string insertQuery = "INSERT INTO Logs (UserId, ActionType, Timestamp) VALUES (@userId, @actionType, @timestamp)";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@actionType", actionType.ToString());
                    command.Parameters.AddWithValue("@timestamp", DateTime.Now);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
