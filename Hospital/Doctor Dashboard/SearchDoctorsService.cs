using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class SearchDoctorsService
    {
        public List<DoctorModel> doctorList { get; private set; }
        private readonly DoctorsDatabaseHelper _doctorDbHelper;

        public SearchDoctorsService(DoctorsDatabaseHelper dbHelper)
        {
            _doctorDbHelper = dbHelper;
            doctorList = new List<DoctorModel>();
        }

        public async Task LoadDoctors(string departmetnOrNamePartialName)
        {
            try
            {
                doctorList.Clear();
                List<DoctorModel> doctorsDepartamentList = await _doctorDbHelper.GetDoctorsByDepartmentPartialName(departmetnOrNamePartialName);
                List<DoctorModel> doctorNameList = await _doctorDbHelper.GetDoctorsByPartialDoctorName(departmetnOrNamePartialName);
                foreach (DoctorModel doctor in doctorsDepartamentList)
                {
                    doctorList.Add(doctor);
                }
                foreach (DoctorModel doctor in doctorNameList)
                {
                    // Check if a doctor with the same ID already exists in the list
                    bool doctorExists = doctorList.Any(d => d.DoctorId == doctor.DoctorId);
                    if (!doctorExists)
                    {
                        doctorList.Add(doctor);
                    }
                }

                // Sort doctors by rating (descending), then by name, then by department
                doctorList = doctorList
                    .OrderByDescending(d => d.Rating)
                    .ThenBy(d => d.DoctorName)
                    .ThenBy(d => d.DepartmentName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading departments: {ex.Message}");
            }
        }

        public List<DoctorModel> GetSearchedDoctors()
        {
            return doctorList;
        }

        // Optional: Add a method to get doctors sorted by different criteria
        public List<DoctorModel> GetDoctorsSortedBy(SortCriteria criteria)
        {
            switch (criteria)
            {
                case SortCriteria.RatingHighToLow:
                    return doctorList.OrderByDescending(d => d.Rating).ToList();
                case SortCriteria.RatingLowToHigh:
                    return doctorList.OrderBy(d => d.Rating).ToList();
                case SortCriteria.NameAZ:
                    return doctorList.OrderBy(d => d.DoctorName).ToList();
                case SortCriteria.NameZA:
                    return doctorList.OrderByDescending(d => d.DoctorName).ToList();
                case SortCriteria.DepartmentAZ:
                    return doctorList.OrderBy(d => d.DepartmentName).ToList();
                case SortCriteria.RatingThenNameThenDepartment:
                    return doctorList
                        .OrderByDescending(d => d.Rating)
                        .ThenBy(d => d.DoctorName)
                        .ThenBy(d => d.DepartmentName)
                        .ToList();
                default:
                    return doctorList;
            }
        }
    }

    // Optional: Add an enum for sort criteria if you want to support multiple sorting options
    public enum SortCriteria
    {
        RatingHighToLow,
        RatingLowToHigh,
        NameAZ,
        NameZA,
        DepartmentAZ,
        RatingThenNameThenDepartment // New composite sorting option
    }
}
