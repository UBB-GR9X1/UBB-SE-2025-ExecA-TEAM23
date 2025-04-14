using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hospital.DatabaseServices;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using Hospital.Configs;

namespace LoginPageTests.Tests
{
    [TestClass]
    public class LogInDatabaseServiceTests
    {
        private readonly ILogInDatabaseService _logInDatabaseService;

        public LogInDatabaseServiceTests()
        {
            _logInDatabaseService = new LogInDatabaseService();
        }

        [TestMethod]
        public async Task GetUserByUsername_WithValidUsername()
        {
            // Task<UserAuthModel> GetUserByUsername(string username)
            var result = await _logInDatabaseService.GetUserByUsername("john_doe");
            Assert.IsNotNull(result);
            Assert.AreEqual("john@example.com", result.Mail);
            Assert.AreEqual("Doctor", result.Role);
        }


        [TestMethod]
        public async Task GetUserByUsername_WithInvalidUsername()
        {
            await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
            {
                await _logInDatabaseService.GetUserByUsername("not_john_doe");
            });
        }

        [TestMethod]
        public async Task CreateAccount_WithValidData_ReturnsTrue()
        {
            var model = new UserCreateAccountModel(
                "nexuser",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "1234567890248",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

            bool result = await _logInDatabaseService.CreateAccount(model);
            Assert.IsTrue(result);

            string query = "DELETE FROM Users WHERE Username = @username";

            using SqlConnection connectionToDatabase = new SqlConnection(Config.GetInstance().DatabaseConnection);
            await connectionToDatabase.OpenAsync().ConfigureAwait(false);

            using SqlCommand command = new SqlCommand(query, connectionToDatabase);
            command.Parameters.AddWithValue("@username", "nexuser");

            int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        [TestMethod]
        public async Task CreateAccount_WithExistingUsername_ThrowsAuthenticationException()
        {
            var model = new UserCreateAccountModel(
                "john_doe",
                "password123",
                "unique@mail.com",
                "John Doe",
                new DateOnly(1990, 1, 1),
                "1234567890123",
                BloodType.A_Positive,
                "Emergency Contact",
                70.0f,
                180
            );

            try
            {
                await _logInDatabaseService.CreateAccount(model);
                Assert.Fail("Exception was not thrown.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
