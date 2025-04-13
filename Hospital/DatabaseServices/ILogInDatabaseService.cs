using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.DatabaseServices
{
    public interface ILogInDatabaseService
    {
        Task<bool> AuthenticationLogService(int userId, ActionType type);
        Task<bool> CreateAccount(UserCreateAccountModel model);
        Task<UserAuthModel> GetUserByUsername(string username);
    }
}