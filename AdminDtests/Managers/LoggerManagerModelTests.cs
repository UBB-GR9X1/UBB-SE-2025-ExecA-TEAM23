using Hospital.Interfaces;
using Hospital.Managers;
using Hospital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Tests.Managers
{
    [TestClass]
    public class LoggerManagerModelTests
    {
        private Mock<ILoggerService> _mockLoggerService;
        private LoggerManagerModel _loggerManager;

        [TestInitialize]
        public void Setup()
        {
            _mockLoggerService = new Mock<ILoggerService>();
            _loggerManager = new LoggerManagerModel(_mockLoggerService.Object);
        }

        [TestMethod]
        public async Task GetAllLogs_ValidRequest_ReturnsAllLogs()
        {
            // Arrange
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, ActionType.LOGIN, DateTime.Now),
                new LogEntryModel(2, 2, ActionType.LOGOUT, DateTime.Now)
            };
            _mockLoggerService.Setup(service => service.GetAllLogs())
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerManager.GetAllLogs();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerService.Verify(service => service.GetAllLogs(), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsByUserId_ValidUserId_ReturnsUserLogs()
        {
            // Arrange
            int userId = 1;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, userId, ActionType.LOGIN, DateTime.Now),
                new LogEntryModel(3, userId, ActionType.LOGOUT, DateTime.Now)
            };
            _mockLoggerService.Setup(service => service.GetLogsByUserId(userId))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerManager.GetLogsByUserId(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerService.Verify(service => service.GetLogsByUserId(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsByActionType_ValidActionType_ReturnsFilteredLogs()
        {
            // Arrange
            var actionType = ActionType.LOGIN;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, actionType, DateTime.Now),
                new LogEntryModel(4, 3, actionType, DateTime.Now)
            };
            _mockLoggerService.Setup(service => service.GetLogsByActionType(actionType))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerManager.GetLogsByActionType(actionType);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerService.Verify(service => service.GetLogsByActionType(actionType), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsBeforeTimestamp_ValidTimestamp_ReturnsFilteredLogs()
        {
            // Arrange
            var timestamp = DateTime.Now;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, ActionType.LOGIN, timestamp.AddDays(-1)),
                new LogEntryModel(3, 2, ActionType.CREATE_ACCOUNT, timestamp.AddHours(-2))
            };
            _mockLoggerService.Setup(service => service.GetLogsBeforeTimestamp(timestamp))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerManager.GetLogsBeforeTimestamp(timestamp);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerService.Verify(service => service.GetLogsBeforeTimestamp(timestamp), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsWithParameters_AllParametersProvided_ReturnsFilteredLogs()
        {
            // Arrange
            int userId = 1;
            var actionType = ActionType.UPDATE_PROFILE;
            var timestamp = DateTime.Now;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(5, userId, actionType, timestamp.AddDays(-1))
            };
            _mockLoggerService.Setup(service => service.GetLogsWithParameters(userId, actionType, timestamp))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerManager.GetLogsWithParameters(userId, actionType, timestamp);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerService.Verify(service => service.GetLogsWithParameters(userId, actionType, timestamp), Times.Once);
        }

        [TestMethod]
        public async Task LogAction_ValidParameters_ReturnsTrue()
        {
            // Arrange
            int userId = 1;
            var actionType = ActionType.LOGIN;
            _mockLoggerService.Setup(service => service.LogAction(userId, actionType))
                .ReturnsAsync(true);

            // Act
            var result = await _loggerManager.LogAction(userId, actionType);

            // Assert
            Assert.IsTrue(result);
            _mockLoggerService.Verify(service => service.LogAction(userId, actionType), Times.Once);
        }
    }
}
