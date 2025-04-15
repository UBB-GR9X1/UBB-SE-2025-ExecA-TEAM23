using Hospital.Configs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services;
using Microsoft.Data.SqlClient;

namespace LoginPageTests;

[TestClass]
public class AuthServiceTests
{
    IAuthService _authService;
    public AuthServiceTests()
    {
        ILogInRepository logInRepository = new LogInRepository();
        _authService = new AuthService(logInRepository);
    }

    [TestMethod]
    public async Task TestCreateAccout_WithValidUser_ReturnsTrue()
    {
        // Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);
        var model = new UserCreateAccountModel(
                "nexuser",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "1900101040500",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

        bool result = await _authService.CreateAccount(model);
        Assert.IsTrue(result);

        string query = "DELETE FROM Users WHERE Username = @username";

        using SqlConnection connectionToDatabase = new SqlConnection(Config.GetInstance().DatabaseConnection);
        await connectionToDatabase.OpenAsync().ConfigureAwait(false);

        using SqlCommand command = new SqlCommand(query, connectionToDatabase);
        command.Parameters.AddWithValue("@username", "nexuser");

        int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    [TestMethod]
    public async Task TestCreateAccout_WithInvalidCNPGender_ThrowsException()
    {
        // Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);
        var model = new UserCreateAccountModel(
                "nexuser",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "5900101040500",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

        await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
        {
            await _authService.CreateAccount(model);
        });
    }

    [TestMethod]
    public async Task TestCreateAccout_WithInvalidCNPYear_ThrowsException()
    {
        // Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);
        var model = new UserCreateAccountModel(
                "nexuser",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "1800101040500",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

        await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
        {
            await _authService.CreateAccount(model);
        });
    }

    [TestMethod]
    public async Task TestCreateAccout_WithInvalidCNPLength_ThrowsException()
    {
        // Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);
        var model = new UserCreateAccountModel(
                "nexuser",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "1800140500",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

        await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
        {
            await _authService.CreateAccount(model);
        });
    }

    [TestMethod]
    public async Task TestCreateAccout_WithInvalidUsername_ThrowsException()
    {
        // Task<bool> CreateAccount(UserCreateAccountModel modelForCreatingUserAccount);
        var model = new UserCreateAccountModel(
                "john_doe",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "1800101040500",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

        await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
        {
            await _authService.CreateAccount(model);
        });
    }

    [TestMethod]
    public async Task TestLoadUserbyUsername_WithValidUsername_ReturnsTrue()
    {
        // Task<bool> LoadUserByUsername(string username);
        var result = await _authService.LoadUserByUsername("john_doe");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task TestLoadUserbyUsername_WithInvalidUsername_ThrowsException()
    {
        // Task<bool> LoadUserByUsername(string username);
        await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
        {
            await _authService.LoadUserByUsername("not_john_doe");
        });
    }

    [TestMethod]
    public async Task VerifyPassword_IncorrectPassword_ReturnsFalse()
    {
        // Task<bool> VerifyPassword(string userInputPassword);
        var result = await _authService.VerifyPassword("wrong_password");
        Assert.IsFalse(result);
    }

}
