using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return await _patientDBService.UpdatePassword(userId, password);
        }

        public async Task<bool> UpdateEmail(int userId, string email)
        {
            return await _patientDBService.UpdateEmail(userId, email);
        }

        public async Task<bool> UpdateUsername(int userId, string username)
        {
            return await _patientDBService.UpdateUsername(userId, username);
        }

        public async Task<bool> UpdateName(int userId, string name)
        {
            return await _patientDBService.UpdateName(userId, name);
        }

        public async Task<bool> UpdateBirthDate(int userId, DateOnly birthDate)
        {
            return await _patientDBService.UpdateBirthDate(userId, birthDate);
        }

        public async Task<bool> UpdateAddress(int userId, string address)
        {
            return await _patientDBService.UpdateAddress(userId, address);
        }

        public async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            return await _patientDBService.UpdatePhoneNumber(userId, phoneNumber);
        }

        public async Task<bool> UpdateEmergencyContact(int userId, string emergencyContact)
        {
            return await _patientDBService.UpdateEmergencyContact(userId, emergencyContact);
        }

        public async Task<bool> UpdateWeight(int userId, float weight)
        {
            return await _patientDBService.UpdateWeight(userId, weight);
        }

        public async Task<bool> UpdateHeight(int userId, int height)
        {
            return await _patientDBService.UpdateHeight(userId, height);
        }


    }
}
