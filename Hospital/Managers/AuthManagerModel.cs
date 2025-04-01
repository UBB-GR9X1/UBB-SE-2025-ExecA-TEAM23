using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.DatabaseServices;
using Hospital.Models;
using Hospital.Exceptions;
using Microsoft.Data.SqlClient;

namespace Hospital.Managers
{
    public class AuthManagerModel
    {
        private readonly LogInDatabaseService _logInDBService;
        public UserAuthModel _userInfo { get; private set; } = UserAuthModel.Default;

        public AuthManagerModel (LogInDatabaseService dbService)
        {
            _logInDBService = dbService;
        }

        public async Task<bool> LoadUserByUsername(string username)
        {
            _userInfo = await _logInDBService.GetUserByUsername(username).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> VerifyPassword(string password)
        {
            if (_userInfo == UserAuthModel.Default)
                return false;

            if (!_userInfo.Password.Equals(password))
                return false;

            return await LogAction(ActionType.LOGIN);
     
        }

        public async Task<bool> Logout()
        {
            if (_userInfo == UserAuthModel.Default)
                throw new AuthenticationException("Not logged in");
           
            bool result = await LogAction(ActionType.LOGOUT);

            if (result)
                _userInfo = UserAuthModel.Default;

            return result;
        }
        public async Task<bool> CreateAccount(UserCreateAccountModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Contains(" "))
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");

            if (string.IsNullOrEmpty(model.Password) || model.Password.Contains(" "))
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");

            if (string.IsNullOrEmpty(model.Mail) || !model.Mail.Contains("@") || !model.Mail.Contains("."))
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");

            if (string.IsNullOrEmpty(model.Name) || !model.Name.Contains(" "))
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");

            if (model.Cnp.Length != 13)
                throw new AuthenticationException("Invalid CNP!\nHas to have length 13");

            foreach (char c in model.Cnp)
            {
                if (!char.IsDigit(c))
                    throw new AuthenticationException("Invalid CNP!\nOnly numbers allowed");
            }

            if (model.EmergencyContact.Length != 10)
                throw new AuthenticationException("Invalid emergency contact!\nIt must have length 10");

            foreach (char c in model.EmergencyContact)
            {
                if (!char.IsDigit(c))
                    throw new AuthenticationException("Invalid emergency contact!\nOnly numbers allowed");
            }

            bool result = await _logInDBService.CreateAccount(model);
            if (result)
                if (await this.LoadUserByUsername(model.Username))
                    return await LogAction(ActionType.LOGIN);

            return result;
        }

        public async Task<bool> LogAction(ActionType action)
        {
            return await _logInDBService.AuthenticationLogService(_userInfo.UserId, action);
        }  
    }
}
