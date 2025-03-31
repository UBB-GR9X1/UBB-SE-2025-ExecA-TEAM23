using Hospital.Exceptions;
using Hospital.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.ViewModels
{
    public class AuthViewModel
    {
        private AuthManagerModel _authManagerModel;

        public AuthViewModel (AuthManagerModel auth)
        {
            _authManagerModel = auth;
        }

        public async Task Login(string username, string password)
        {
            bool userExists = await _authManagerModel.LoadUserByUsername(username);

            if (!userExists)
                throw new AuthenticationException("Username doesn't exist!");

            bool validPassword = await _authManagerModel.VerifyPassword(password);

            if (!validPassword)
                throw new AuthenticationException("Invalid password!");
        }

        public async Task Logout()
        {
            await _authManagerModel.Logout();
        }

        public async Task CreateAccount(string username, string password, string mail, string name, DateOnly birthDate, string cnp)
        {
            await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp);
        }
    }
}
