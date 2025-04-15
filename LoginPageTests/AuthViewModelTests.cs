using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.ViewModels;

namespace LoginPageTests;

[TestClass]
public class AuthViewModelTests
{
    AuthViewModel authViewModel;
    public AuthViewModelTests()
    {
        ILogInDatabaseService logInDatabaseService = new LogInDatabaseService();
        AuthManagerModel authManagerModel = new AuthManagerModel(logInDatabaseService);
        authViewModel = new AuthViewModel(authManagerModel);
    }

    /* [TestMethod]
    public async Task TestLogin_withValidUser_ReturnsTrue()
    {
        // Task<bool> Login(string username, string password)
        await authViewModel.Login("john_doe", "john1234");
    } */
}
