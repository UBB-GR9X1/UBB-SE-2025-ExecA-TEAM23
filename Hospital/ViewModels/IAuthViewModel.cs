using Hospital.Managers;
using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public interface IAuthViewModel
    {
        IAuthManagerModel _authManagerModel { get; }

        Task CreateAccount(UserCreateAccountModel model);
        Task Login(string username, string password);
        Task Logout();
    }
}