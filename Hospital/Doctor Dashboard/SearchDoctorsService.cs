using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Doctor_Dashboard;

namespace Hospital.Managers
{
    public class SearchDoctorsService : ISearchDoctorsService
    {
        public List<DoctorModel> AvailableDoctors { get; private set; }
        private readonly IDoctorsDatabaseHelper _doctorDatabaseHelper;

        public SearchDoctorsService(IDoctorsDatabaseHelper doctorDatabaseHelper)
        {
            _doctorDatabaseHelper = doctorDatabaseHelper;
            AvailableDoctors = new List<DoctorModel>();
        }

        public async Task LoadDoctors(string searchTerm)
        {
            try
            {
                AvailableDoctors.Clear();

                // Search by department name
                List<DoctorModel> doctorsByDepartment = await _doctorDatabaseHelper.GetDoctorsByDepartmentPartialName(searchTerm);

                // Search by doctor name
                List<DoctorModel> doctorsByName = await _doctorDatabaseHelper.GetDoctorsByPartialDoctorName(searchTerm);

                // Add doctors from department search
                foreach (DoctorModel doctor in doctorsByDepartment)
                {
                    AvailableDoctors.Add(doctor);
                }

                // Add doctors from name search if not already in list
                foreach (DoctorModel doctor in doctorsByName)
                {
                    bool isDoctorAlreadyInList = AvailableDoctors.Any(existingDoctor => existingDoctor.DoctorId == doctor.DoctorId);
                    if (!isDoctorAlreadyInList)
                    {
                        AvailableDoctors.Add(doctor);
                    }
                }

                // Sort doctors by multiple criteria
                AvailableDoctors = SortDoctorsByDefaultCriteria(AvailableDoctors);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error loading doctors: {exception.Message}");
            }
        }

        public List<DoctorModel> GetSearchedDoctors()
        {
            return AvailableDoctors;
        }

        // Method to sort doctors by different criteria
        public List<DoctorModel> GetDoctorsSortedBy(SortCriteria sortCriteria)
        {
            switch (sortCriteria)
            {
                case SortCriteria.RatingHighToLow:
                    return AvailableDoctors.OrderByDescending(doctor => doctor.Rating).ToList();

                case SortCriteria.RatingLowToHigh:
                    return AvailableDoctors.OrderBy(doctor => doctor.Rating).ToList();

                case SortCriteria.NameAscending:
                    return AvailableDoctors.OrderBy(doctor => doctor.DoctorName).ToList();

                case SortCriteria.NameDescending:
                    return AvailableDoctors.OrderByDescending(doctor => doctor.DoctorName).ToList();

                case SortCriteria.DepartmentAscending:
                    return AvailableDoctors.OrderBy(doctor => doctor.DepartmentName).ToList();

                case SortCriteria.RatingThenNameThenDepartment:
                    return SortDoctorsByDefaultCriteria(AvailableDoctors);

                default:
                    return AvailableDoctors;
            }
        }

        private List<DoctorModel> SortDoctorsByDefaultCriteria(List<DoctorModel> doctorsToSort)
        {
            return doctorsToSort
                .OrderByDescending(doctor => doctor.Rating)
                .ThenBy(doctor => doctor.DoctorName)
                .ThenBy(doctor => doctor.DepartmentName)
                .ToList();
        }
    }

    // Enum for doctor sorting criteria
    public enum SortCriteria
    {
        RatingHighToLow,
        RatingLowToHigh,
        NameAscending,
        NameDescending,
        DepartmentAscending,
        RatingThenNameThenDepartment
    }
}