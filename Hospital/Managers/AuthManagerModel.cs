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
            try
            {
                _userInfo = await _logInDBService.GetUserByUsername(username).ConfigureAwait(false);
                return true;
            }
            catch (Exception e) when (e is AuthenticationException || e is SqlException)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                _userInfo = UserAuthModel.Default;
                return false;
            }
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

        public async Task<bool> LogAction(ActionType action)
        {
            try
            {
                return await _logInDBService.AuthenticationLogService(_userInfo.UserId, action);
            }

            catch (AuthenticationException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
            
    }
}
