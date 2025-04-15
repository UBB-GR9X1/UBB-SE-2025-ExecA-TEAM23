using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<DoctorModel>> GetDoctorsByDepartmentPartialName(string departmentPartialName);

        Task<List<DoctorModel>> GetDoctorsByPartialDoctorName(string doctorPartialName);

        Task<List<DoctorJointModel>> GetDoctorsByDepartment(int departmentId);

        Task<List<DoctorJointModel>> GetAllDoctors();

        Task<DoctorModel> GetDoctorById(int doctorId);

        Task<bool> UpdateDoctorName(int userId, string name);

        Task<bool> UpdateDoctorEmail(int userId, string email);

        Task<bool> UpdateDoctorCareerInfo(int userId, string careerInfo);

        Task<bool> UpdateDoctorDepartment(int userId, int departmentId);

        Task<bool> UpdateDoctorRating(int userId, double rating);

        Task<bool> UpdateDoctorAvatarUrl(int userId, string newAvatarUrl);

        Task<bool> UpdateDoctorPhoneNumber(int userId, string newPhoneNumber);

        Task<bool> UpdateLogService(int userId, ActionType type);
    }
}