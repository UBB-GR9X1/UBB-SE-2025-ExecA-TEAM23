using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class SearchDoctorsManagerModel
    {
        public List<DoctorDisplayModel> doctorList { get; private set; }
        private DoctorsDatabaseService _doctorDBService;

        public SearchDoctorsManagerModel(DoctorsDatabaseService dbService)
        {
            _doctorDBService = dbService;
            doctorList = new List<DoctorDisplayModel>();
        }

        public async Task LoadDoctors(string departmetnOrNamePartialName)
        {
            try
            {
                doctorList.Clear();
                List<DoctorDisplayModel> doctorsDepartamentList = await _doctorDBService.GetDoctorsByDepartmentPartialName(departmetnOrNamePartialName);
                List<DoctorDisplayModel> doctorNameList = await _doctorDBService.GetDoctorsByPartialDoctorName(departmetnOrNamePartialName);
                foreach (DoctorDisplayModel doctor in doctorsDepartamentList)
                {
                    doctorList.Add(doctor);
                }
                foreach (DoctorDisplayModel doctor in doctorNameList)
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

        public List<DoctorDisplayModel> GetSearchedDoctors()
        {
            return doctorList;
        }

        // Optional: Add a method to get doctors sorted by different criteria
        public List<DoctorDisplayModel> GetDoctorsSortedBy(SortCriteria criteria)
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
