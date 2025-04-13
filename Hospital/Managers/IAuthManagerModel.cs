using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public interface IAuthManagerModel
    {
        UserAuthModel _userInfo { get; }

        Task<bool> CreateAccount(UserCreateAccountModel model);
        Task<bool> LoadUserByUsername(string username);
        Task<bool> LogAction(ActionType action);
        Task<bool> Logout();
        Task<bool> VerifyPassword(string password);
    }
}