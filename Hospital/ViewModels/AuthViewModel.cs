using Hospital.Exceptions;
using Hospital.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class AuthViewModel
    {
        private AuthManagerModel _authManagerModel;

        public AuthViewModel (AuthManagerModel auth)
        {
            _authManagerModel = auth;
        }

        public async Task<bool> Login(string username, string password)
        {
            bool userExists = await _authManagerModel.LoadUserByUsername(username);
            if (!userExists)
                throw new AuthenticationException("Username doesn't exist!");
            bool validPassword = _authManagerModel.VerifyPassword(password);
            if (!validPassword)
                throw new AuthenticationException("Invalid password!");
            return true;
        }

        public void Logout()
        {
            try
            {
                _authManagerModel.Logout();
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
