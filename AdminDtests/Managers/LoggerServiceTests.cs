using Hospital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Repositories;
using Hospital.Services;

namespace Hospital.Tests.Managers
{
    [TestClass]
    public class LoggerServiceTests
    {
        private Mock<ILoggerRepository> _mockLoggerRepository;
        private LoggerService _loggerService;

        [TestInitialize]
        public void Setup()
        {
            _mockLoggerRepository = new Mock<ILoggerRepository>();
            _loggerService = new LoggerService(_mockLoggerRepository.Object);
        }

        [TestMethod]
        public async Task GetAllLogs_GetsAllLogs_ReturnsAllLogs()
        {
            // Arrange
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, ActionType.LOGIN, DateTime.Now),
                new LogEntryModel(2, 2, ActionType.LOGOUT, DateTime.Now)
            };
            _mockLoggerRepository.Setup(service => service.GetAllLogs())
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerService.GetAllLogs();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerRepository.Verify(service => service.GetAllLogs(), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsByUserId_GetLogsByUserId_ReturnsUserLogs()
        {
            // Arrange
            int userId = 1;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, userId, ActionType.LOGIN, DateTime.Now),
                new LogEntryModel(3, userId, ActionType.LOGOUT, DateTime.Now)
            };
            _mockLoggerRepository.Setup(service => service.GetLogsByUserId(userId))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerService.GetLogsByUserId(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerRepository.Verify(service => service.GetLogsByUserId(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsByActionType_GetLogsByActionType_ReturnsFilteredLogs()
        {
            // Arrange
            var actionType = ActionType.LOGIN;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, actionType, DateTime.Now),
                new LogEntryModel(4, 3, actionType, DateTime.Now)
            };
            _mockLoggerRepository.Setup(service => service.GetLogsByActionType(actionType))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerService.GetLogsByActionType(actionType);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerRepository.Verify(service => service.GetLogsByActionType(actionType), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsBeforeTimestamp_GetLogsBeforeTimestamp_ReturnsFilteredLogs()
        {
            // Arrange
            var timestamp = DateTime.Now;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, ActionType.LOGIN, timestamp.AddDays(-1)),
                new LogEntryModel(3, 2, ActionType.CREATE_ACCOUNT, timestamp.AddHours(-2))
            };
            _mockLoggerRepository.Setup(service => service.GetLogsBeforeTimestamp(timestamp))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerService.GetLogsBeforeTimestamp(timestamp);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerRepository.Verify(service => service.GetLogsBeforeTimestamp(timestamp), Times.Once);
        }

        [TestMethod]
        public async Task GetLogsWithParameter_GetLogsWithParametersAndFilterLogs_ReturnsFilteredLogs()
        {
            // Arrange
            int userId = 1;
            var actionType = ActionType.UPDATE_PROFILE;
            var timestamp = DateTime.Now;
            var expectedLogs = new List<LogEntryModel>
            {
                new LogEntryModel(5, userId, actionType, timestamp.AddDays(-1))
            };
            _mockLoggerRepository.Setup(service => service.GetLogsWithParameters(userId, actionType, timestamp))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _loggerService.GetLogsWithParameters(userId, actionType, timestamp);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogs.Count, result.Count);
            _mockLoggerRepository.Verify(service => service.GetLogsWithParameters(userId, actionType, timestamp), Times.Once);
        }

        [TestMethod]
        public async Task LogAction_ChecksLogAction_ReturnsTrue()
        {
            // Arrange
            int userId = 1;
            var actionType = ActionType.LOGIN;
            _mockLoggerRepository.Setup(service => service.LogAction(userId, actionType))
                .ReturnsAsync(true);

            // Act
            var result = await _loggerService.LogAction(userId, actionType);

            // Assert
            Assert.IsTrue(result);
            _mockLoggerRepository.Verify(service => service.LogAction(userId, actionType), Times.Once);
        }
    }
}
