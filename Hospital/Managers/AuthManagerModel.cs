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
        public static UserAuthModel? s_userInfo { get; private set; } = null;

        public AuthManagerModel (LogInDatabaseService dbService)
        {
            _logInDBService = dbService;
        }

        public async Task<bool> LoadUserByUsername(string username)
        {
            try
            {
                s_userInfo = await _logInDBService.GetUserByUsername(username).ConfigureAwait(false);
                return true;
            }
            catch (AuthenticationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (SqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return false;
        }

        public bool VerifyPassword(string password)
        {
            if (s_userInfo != null)
                return s_userInfo.Password.Equals(password) ? true : false;
            return false;
        }

        public void Logout()
        {
            if (s_userInfo == null)
                throw new AuthenticationException("Not logged in");
            s_userInfo = null;
        }
    }
}
