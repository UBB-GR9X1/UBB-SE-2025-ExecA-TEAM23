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
    public class PatientManagerModel
    {
        private readonly PatientsDatabaseService _patientDBService;

        //Use this if you want to work on a specific patient
        public PatientJointModel _patientInfo { get; private set; }

        //Use this if you want to work with more patients
        public List<PatientJointModel> _patientList { get; private set; }
        public PatientManagerModel()
        {
            _patientDBService = new PatientsDatabaseService();
        }

        public async Task<bool> LoadPatientInfoByUserId(int userId)
        {
            _patientInfo = await _patientDBService.GetPatientByUserId(userId).ConfigureAwait(false);
            Debug.WriteLine($"Patient info loaded: {_patientInfo.PatientName}");
            return true;
        }
        public async Task<bool> LoadAllPatients()
        {
            _patientList = await _patientDBService.GetAllPatients().ConfigureAwait(false);
            return true;
        }
        public async Task<bool> UpdatePassword(int userId, string password)
        {
            if (string.IsNullOrEmpty(password) || password.Contains(' '))
                throw new InputProfileException("Invalid password!\nCan't be null or with space");

            if (password.Length > 255)
                throw new InputProfileException("Invalid password!\nCan't be more than 255 characters");

            return await _patientDBService.UpdatePassword(userId, password);
        }

        public async Task<bool> UpdateEmail(int userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
                throw new InputProfileException("Invalid email format.");
            return await _patientDBService.UpdateEmail(userId, email);
        }

        public async Task<bool> UpdateUsername(int userId, string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Contains(' '))
                throw new InputProfileException("Invalid username!\nCan't be null or with space");

            if (username.Length > 50)
                throw new InputProfileException("Invalid username!\nCan't be more than 50 characters");

            return await _patientDBService.UpdateUsername(userId, username);
        }

        public async Task<bool> UpdateName(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit))
                throw new InputProfileException("Name cannot be empty.");

            return await _patientDBService.UpdateName(userId, name);
        }

        public async Task<bool> UpdateBirthDate(int userId, DateOnly birthDate)
        {
            return await _patientDBService.UpdateBirthDate(userId, birthDate);
        }

        public async Task<bool> UpdateAddress(int userId, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                address = "";

            return await _patientDBService.UpdateAddress(userId, address);
        }

        public async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            if (phoneNumber.Length != 10)
                throw new InputProfileException("Invalid phone number!\nIt must have length 10");

            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                    throw new InputProfileException("Invalid phone number!\nOnly numbers allowed");
            }

            return await _patientDBService.UpdatePhoneNumber(userId, phoneNumber);
        }

        public async Task<bool> UpdateEmergencyContact(int userId, string emergencyContact)
        {
            if (emergencyContact.Length != 10)
                throw new InputProfileException("Invalid emergency contact!\nIt must have length 10");

            foreach (char c in emergencyContact)
            {
                if (!char.IsDigit(c))
                    throw new InputProfileException("Invalid emergency contact!\nOnly numbers allowed");
            }

            return await _patientDBService.UpdateEmergencyContact(userId, emergencyContact);
        }

        public async Task<bool> UpdateWeight(int userId, double weight)
        {
            if (weight <= 0)
                throw new InputProfileException("Weight must be greater than 0");

            return await _patientDBService.UpdateWeight(userId, weight);
        }

        public async Task<bool> UpdateHeight(int userId, int height)
        {
            if (height <= 0)
                throw new InputProfileException("Height must be greater than 0.");

            return await _patientDBService.UpdateHeight(userId, height);
        }
    }
}
