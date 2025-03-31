using Hospital.Exceptions;
using Hospital.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;
using System.Text.RegularExpressions;

namespace Hospital.ViewModels
{
    public class AuthViewModel
    {
        public AuthManagerModel _authManagerModel { get; private set; }

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

        public async Task CreateAccount(string username, string password, string mail, string name, DateOnly birthDate, string cnp, BloodType bloodType, string emergencyContact, double weight, int height)
        {
            switch (bloodType)
            {
                case BloodType.A_Positive:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "A+", emergencyContact, weight, height);
                    break;
                case BloodType.A_Negative:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "A-", emergencyContact, weight, height);
                    return;
                case BloodType.B_Positive:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "B+", emergencyContact, weight, height);
                    return;
                case BloodType.B_Negative:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "B-", emergencyContact, weight, height);
                    return;
                case BloodType.AB_Positive:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "AB+", emergencyContact, weight, height);
                    return;
                case BloodType.AB_Negative:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "AB-", emergencyContact, weight, height);
                    return;
                case BloodType.O_Positive:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "O+", emergencyContact, weight, height);
                    return;
                case BloodType.O_Negative:
                    await _authManagerModel.CreateAccount(username, password, mail, name, birthDate, cnp, "O-", emergencyContact, weight, height);
                    return;
            }
        }
    }
}
