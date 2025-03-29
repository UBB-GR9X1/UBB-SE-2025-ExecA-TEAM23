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
        public static UserAuthModel? userInfo { get; private set; } = null;

        public AuthManagerModel (LogInDatabaseService dbService)
        {
            _logInDBService = dbService;
        }

        public async Task<bool> LoadUserByUsername(string username)
        {
            try
            {
                userInfo = await _logInDBService.GetUserByUsername(username).ConfigureAwait(false);
                return true;
            }
            catch (AuthenticationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
            catch (SqlException e)
            {
                throw new Exception($"SQL Exception: {e.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Error loading Users: {e.Message}");
            }
        }

        public bool VerifyPassword(string password)
        {
            if (userInfo != null)
                return userInfo.Password.Equals(password) ? true : false;
            return false;
        }
    }
}
