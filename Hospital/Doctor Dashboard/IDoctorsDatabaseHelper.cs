using Hospital.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.DatabaseServices
{
    public interface IDoctorsDatabaseHelper
    {
        Task<List<DoctorModel>> GetDoctorsByDepartmentPartialName(string departmentPartialName);
        Task<List<DoctorModel>> GetDoctorsByPartialDoctorName(string doctorPartialName);
    }
}