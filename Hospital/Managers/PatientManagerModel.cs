using Hospital.DatabaseServices;
using Hospital.Exceptions;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class PatientManagerModel : IPatientManagerModel
    {
        private readonly IPatientsDatabaseService _patientsDatabaseService;

        //Use this for working on a specific patient
        public PatientJointModel _patientInfo { get; private set; } = PatientJointModel.Default;

        //Use this for working with more patients
        public List<PatientJointModel> _patientList { get; private set; } = new List<PatientJointModel>();

        public PatientManagerModel() : this(new PatientsDatabaseService()) { }

        // Second constructor for test injection
        public PatientManagerModel(IPatientsDatabaseService testService)
        {
            _patientsDatabaseService = testService;
        }


        public async Task<bool> LoadPatientInfoByUserId(int userId)
        {
            _patientInfo = await _patientsDatabaseService.GetPatientByUserId(userId).ConfigureAwait(false);
            Debug.WriteLine($"Patient info loaded: {_patientInfo.PatientName}");
            return true;
        }

        public async Task<bool> LoadAllPatients()
        {
            _patientList = await _patientsDatabaseService.GetAllPatients().ConfigureAwait(false);
            return true;
        }

        public virtual async Task<bool> UpdatePassword(int userId, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Contains(' '))
                throw new InputProfileException("Invalid password!\nPassword cannot be empty or contain spaces.");

            if (password.Length > 255)
                throw new InputProfileException("Invalid password!\nPassword cannot exceed 255 characters.");

            return await _patientsDatabaseService.UpdatePassword(userId, password);
        }

        public virtual async Task<bool> UpdateEmail(int userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
                throw new InputProfileException("Invalid email format.");

            if (email.Length > 100)
                throw new InputProfileException("Invalid email!\nEmail cannot exceed 100 characters.");

            return await _patientsDatabaseService.UpdateEmail(userId, email);
        }

        public virtual async Task<bool> UpdateUsername(int userId, string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Contains(' '))
                throw new InputProfileException("Invalid username!\nUsername cannot be empty or contain spaces.");

            if (username.Length > 50)
                throw new InputProfileException("Invalid username!\nUsername cannot exceed 50 characters.");

            return await _patientsDatabaseService.UpdateUsername(userId, username);
        }

        public virtual async Task<bool> UpdateName(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit))
                throw new InputProfileException("Name cannot be empty or contain digits.");

            if (name.Length > 100)
                throw new InputProfileException("Invalid name!\nName cannot exceed 100 characters.");

            return await _patientsDatabaseService.UpdateName(userId, name);
        }

        public virtual async Task<bool> UpdateBirthDate(int userId, DateOnly birthDate)
        {
            return await _patientsDatabaseService.UpdateBirthDate(userId, birthDate);
        }

        public virtual async Task<bool> UpdateAddress(int userId, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                address = "";

            if (address.Length > 255)
                throw new InputProfileException("Invalid address!\nAddress cannot exceed 255 characters.");

            return await _patientsDatabaseService.UpdateAddress(userId, address);
        }

        public virtual async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            if (phoneNumber.Length != 10)
                throw new InputProfileException("Invalid phone number!\nPhone number must be 10 digits long.");

            if (!phoneNumber.All(char.IsDigit))
                throw new InputProfileException("Invalid phone number!\nOnly digits are allowed.");

            return await _patientsDatabaseService.UpdatePhoneNumber(userId, phoneNumber);
        }

        public virtual async Task<bool> UpdateEmergencyContact(int userId, string emergencyContact)
        {
            if (emergencyContact.Length != 10)
                throw new InputProfileException("Invalid emergency contact!\nContact number must be 10 digits long.");

            if (!emergencyContact.All(char.IsDigit))
                throw new InputProfileException("Invalid emergency contact!\nOnly digits are allowed.");

            return await _patientsDatabaseService.UpdateEmergencyContact(userId, emergencyContact);
        }

        public virtual async Task<bool> UpdateWeight(int userId, double weight)
        {
            if (weight <= 0)
                throw new InputProfileException("Invalid weight!\nWeight must be greater than 0.");

            return await _patientsDatabaseService.UpdateWeight(userId, weight);
        }

        public virtual async Task<bool> UpdateHeight(int userId, int height)
        {
            if (height <= 0)
                throw new InputProfileException("Invalid height!\nHeight must be greater than 0.");

            return await _patientsDatabaseService.UpdateHeight(userId, height);
        }

        public virtual async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await _patientsDatabaseService.LogUpdate(userId, action);
        }
    }
}
