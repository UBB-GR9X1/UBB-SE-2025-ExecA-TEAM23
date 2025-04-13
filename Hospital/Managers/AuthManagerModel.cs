using System;
using System.Threading.Tasks;
using Hospital.DatabaseServices;
using Hospital.Models;
using Hospital.Exceptions;
using Windows.Services.Maps;

namespace Hospital.Managers
{
    public class AuthManagerModel : IAuthManagerModel
    {
        private readonly ILogInDatabaseService _logInDBService;
        public UserAuthModel _userInfo { get; private set; } = UserAuthModel.Default;

        public AuthManagerModel(ILogInDatabaseService dbService)
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
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Contains(' '))
                throw new AuthenticationException("Invalid username!\nCan't be null or with space");

            if (model.Username.Length > 50)
                throw new AuthenticationException("Invalid username!\nCan't be more than 50 characters");

            if (string.IsNullOrEmpty(model.Password) || model.Password.Contains(' '))
                throw new AuthenticationException("Invalid password!\nCan't be null or with space");

            if (model.Password.Length > 255)
                throw new AuthenticationException("Invalid password!\nCan't be more than 255 characters");

            if (string.IsNullOrEmpty(model.Mail) || !model.Mail.Contains('@') || !model.Mail.Contains('.'))
                throw new AuthenticationException("Invalid mail!\nCan't be null, has to contain '@' and '.'");

            if (model.Mail.Length > 100)
                throw new AuthenticationException("Invalid mail!\nCan't be more than 100 characters");

            if (string.IsNullOrEmpty(model.Name) || !model.Name.Contains(' '))
                throw new AuthenticationException("Invalid name!\nCan't be null, has to contain space");

            if (model.Name.Length > 100)
                throw new AuthenticationException("Invalid name!\nCan't be more than 100 characters");

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

            // check if model is at least 14 years old
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minValidDate = todayDate.AddYears(-14);
            if (model.BirthDate > minValidDate)
                throw new AuthenticationException("Invalid Date\nPatient must be at least 14 years old!");

            // check if valid gender
            switch (model.BirthDate.Year <= 1999)
            {
                case true:
                    if (model.Cnp[0] != '1' && model.Cnp[0] != '2')
                        throw new AuthenticationException("CNP gender is invalid");
                    break;
                case false:
                    if (model.Cnp[0] != '5' && model.Cnp[0] != '6')
                        throw new AuthenticationException("CNP gender is invalid");
                    break;
            }

            if (model.BirthDate.Year.ToString().Length != 4)
                throw new AuthenticationException("CNP birth year errorYou may be old, but you surely aren't this old :)!");

            // check if valid match between birth date and CNP birth date
            if (model.BirthDate.Year.ToString().Substring(2, 2) != model.Cnp.Substring(1, 2))
                throw new AuthenticationException("Mismatch between Birth year and CNP birth year");
            if (model.BirthDate.Month.ToString().Length == 1)
            {
                if (model.BirthDate.Month.ToString()[0] != model.Cnp[4] || model.Cnp[3] != '0')
                    throw new AuthenticationException("Mismatch between Birth Month and CNP birth month");
            }

            else
                if (model.BirthDate.Month.ToString() != model.Cnp.Substring(3, 2))
                throw new AuthenticationException("Mismatch between Birth Month and CNP birth month");

            if (model.BirthDate.Day.ToString().Length == 1)
            {
                if (model.BirthDate.Day.ToString()[0] != model.Cnp[6] || model.Cnp[5] != '0')
                    throw new AuthenticationException("Mismatch between Birth Day and CNP birth day");
            }
            else
                if (model.BirthDate.Day.ToString() != model.Cnp.Substring(5, 2))
                throw new AuthenticationException("Mismatch between Birth Day and CNP birth day");

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
