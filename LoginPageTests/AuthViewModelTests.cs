using Hospital.Repositories;
using Hospital.Services;
using Hospital.ViewModels;

namespace LoginPageTests;

[TestClass]
public class AuthViewModelTests
{
    AuthViewModel authViewModel;
    public AuthViewModelTests()
    {
        ILogInRepository logInRepository = new LogInRepository();
        AuthService authService = new AuthService(logInRepository);
        authViewModel = new AuthViewModel(authService);
    }

    /* [TestMethod]
    public async Task TestLogin_withValidUser_ReturnsTrue()
    {
        // Task<bool> Login(string username, string password)
        await authViewModel.Login("john_doe", "john1234");
    } */
}
