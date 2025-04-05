using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class DoctorManagerModel
    {
        private readonly DoctorsDatabaseService _doctorDBService;
        public DoctorDisplayModel _doctorInfo { get; private set; } = DoctorDisplayModel.Default;
        public List<DoctorDisplayModel> _doctorList { get; private set; }

        public DoctorManagerModel(DoctorsDatabaseService doctorDBService)
        {
            _doctorDBService = doctorDBService;
            _doctorList = new List<DoctorDisplayModel>();
        }

        public async Task<List<DoctorJointModel>> GetDoctorsByDepartment(int departmentId)
        {
            return await _doctorDBService.GetDoctorsByDepartment(departmentId);
        }

        public async Task<List<DoctorJointModel>> GetAllDoctors()
        {
            return await _doctorDBService.GetAllDoctors();
        }

        public async Task<bool> LoadDoctorInfoByUserId(int doctorId)
        {
            try
            {
                _doctorInfo = await _doctorDBService.GetDoctorById(doctorId);

                if (_doctorInfo != DoctorDisplayModel.Default)
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
            _doctorList = await _doctorDBService.GetDoctorsByDepartmentPartialName(departmentPartialName);
            return _doctorList != null;
        }

        public async Task<bool> SearchDoctorsByName(string namePartial)
        {
            _doctorList = await _doctorDBService.GetDoctorsByPartialDoctorName(namePartial);
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
            return await _doctorDBService.UpdateDoctorName(userId, name);
        }

        public async Task<bool> UpdateDepartment(int userId, int departmentId)
        {
            return await _doctorDBService.UpdateDoctorDepartment(userId, departmentId);
        }

        public async Task<bool> UpdateRating(int userId, double rating)
        {
            if (rating < 0.0 || rating > 5.0)
            {
                throw new Exception("Rating must be between 0 and 5");
            }

            return await _doctorDBService.UpdateDoctorRating(userId, rating);
        }

        public async Task<bool> UpdateCareerInfo(int userId, string careerInfo)
        {
            if (careerInfo != null && careerInfo.Length > int.MaxValue)
            {
                throw new Exception("Career info is too long");
            }

            careerInfo ??= "";

            return await _doctorDBService.UpdateDoctorCareerInfo(userId, careerInfo);
        }

        public async Task<bool> UpdateAvatarUrl(int userId, string avatarUrl)
        {
            if (avatarUrl != null)
            {
                if (avatarUrl.Length > 255)
                {
                    throw new Exception("Avatar URL is too long");
                }

                if (!Uri.IsWellFormedUriString(avatarUrl, UriKind.Absolute))
                {
                    throw new Exception("Invalid avatar URL format");
                }
            }

            avatarUrl ??= "";

            return await _doctorDBService.UpdateDoctorAvatarUrl(userId, avatarUrl);
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

            return await _doctorDBService.UpdateDoctorPhoneNumber(userId, phoneNumber);
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

            return await _doctorDBService.UpdateDoctorEmail(userId, email);
        }
        public async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await _doctorDBService.UpdateLogService(userId, action);
        }
    }
}