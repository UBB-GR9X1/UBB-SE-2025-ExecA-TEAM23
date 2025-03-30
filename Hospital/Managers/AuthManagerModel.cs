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
        public async Task<bool> CreateAccount(string username, string password, string mail, string name, DateOnly birthDate, string cnp)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Contains(" "))
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");

            if (string.IsNullOrEmpty(password) || password.Contains(" "))
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");

            if (string.IsNullOrEmpty(mail) || !mail.Contains("@") || !mail.Contains("."))
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");

            if (string.IsNullOrEmpty(name) || !name.Contains(" "))
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");

            if (cnp.Length != 13)
                throw new AuthenticationException("Invalid CNP!\nHas to have length 13");

            foreach (char c in cnp)
            {
                if (!char.IsDigit(c))
                    throw new AuthenticationException("Invalid CNP!\nOnly numbers allowed");
            }

            bool result = await _logInDBService.CreateAccount(username, password, mail, name, birthDate, cnp);
            if (result)
                return await this.LoadUserByUsername(username);
            return result;
        }

        public async Task<bool> LogAction(ActionType action)
        {
            return await _logInDBService.AuthenticationLogService(_userInfo.UserId, action);
        }  
    }
}
