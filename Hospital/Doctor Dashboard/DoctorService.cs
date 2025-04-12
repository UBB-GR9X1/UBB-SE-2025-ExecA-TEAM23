using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class DoctorService
    {
        private readonly DoctorsDatabaseHelper _doctorDbHelper;
        public DoctorModel _doctorInfo { get; private set; } = DoctorModel.Default;
        public List<DoctorModel> _doctorList { get; private set; }

        public DoctorService(DoctorsDatabaseHelper doctorDbHelper)
        {
            _doctorDbHelper = doctorDbHelper;
            _doctorList = new List<DoctorModel>();
        }

        public async Task<List<DoctorJointModel>> GetDoctorsByDepartment(int departmentId)
        {
            return await _doctorDbHelper.GetDoctorsByDepartment(departmentId);
        }

        public async Task<List<DoctorJointModel>> GetAllDoctors()
        {
            return await _doctorDbHelper.GetAllDoctors();
        }

        public async Task<bool> LoadDoctorInfoByUserId(int doctorId)
        {
            try
            {
                _doctorInfo = await _doctorDbHelper.GetDoctorById(doctorId);

                if (_doctorInfo != DoctorModel.Default)
                {
                    return true;
                }
                throw new Exception($"No doctor found for user ID: {doctorId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading doctor info: {ex.Message}");
            }
        }

        public async Task<bool> SearchDoctorsByDepartment(string departmentPartialName)
        {
            _doctorList = await _doctorDbHelper.GetDoctorsByDepartmentPartialName(departmentPartialName);
            return _doctorList != null;
        }

        public async Task<bool> SearchDoctorsByName(string namePartial)
        {
            _doctorList = await _doctorDbHelper.GetDoctorsByPartialDoctorName(namePartial);
            return _doctorList != null;
        }

        public async Task<bool> UpdateDoctorName(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !name.Contains(' '))
            {
                throw new Exception("Doctor name cannot be empty and has to contain space");
            }

            if (name.Length > 100)
            {
                throw new Exception("Doctor name is too long");
            }
            return await _doctorDbHelper.UpdateDoctorName(userId, name);
        }

        public async Task<bool> UpdateDepartment(int userId, int departmentId)
        {
            return await _doctorDbHelper.UpdateDoctorDepartment(userId, departmentId);
        }

        public async Task<bool> UpdateRating(int userId, double rating)
        {
            if (rating < 0.0 || rating > 5.0)
            {
                throw new Exception("Rating must be between 0 and 5");
            }

            return await _doctorDbHelper.UpdateDoctorRating(userId, rating);
        }

        public async Task<bool> UpdateCareerInfo(int userId, string careerInfo)
        {
            if (careerInfo != null && careerInfo.Length > int.MaxValue)
            {
                throw new Exception("Career info is too long");
            }

            careerInfo ??= "";

            return await _doctorDbHelper.UpdateDoctorCareerInfo(userId, careerInfo);
        }

        public async Task<bool> UpdateAvatarUrl(int userId, string avatarUrl)
        {
            if (avatarUrl != null)
            {
                if (avatarUrl.Length > 255)
                {
                    throw new Exception("Avatar URL is too long");
                }
            }

            avatarUrl ??= "";

            return await _doctorDbHelper.UpdateDoctorAvatarUrl(userId, avatarUrl);
        }

        public async Task<bool> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            if (phoneNumber != null)
            {
                if (phoneNumber.Length != 10)
                {
                    throw new Exception("Phone number must have length 10");
                }

                foreach (char c in phoneNumber)
                {
                    if (!char.IsDigit(c))
                        throw new Exception("Phone numbers must contain only digits");
                }
            }

            phoneNumber ??= "";

            return await _doctorDbHelper.UpdateDoctorPhoneNumber(userId, phoneNumber);
        }

        public async Task<bool> UpdateEmail(int userId, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email cannot be empty");
            }

            if (email.Length > 100)
            {
                throw new Exception("Email is too long");
            }

            if (!email.Contains('@') || !email.Contains('.'))
                throw new Exception("Invalid email format!\nNeeds to have @ and .");

            return await _doctorDbHelper.UpdateDoctorEmail(userId, email);
        }
        public async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await _doctorDbHelper.UpdateLogService(userId, action);
        }
    }
}