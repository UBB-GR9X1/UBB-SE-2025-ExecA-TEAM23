using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class SearchDoctorsService : ISearchDoctorsService
    {
        private readonly IDoctorsDatabaseHelper _doctorDatabaseHelper;

        public List<DoctorModel> AvailableDoctors { get; private set; }

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

                var doctorsByDepartment = await _doctorDatabaseHelper.GetDoctorsByDepartmentPartialName(searchTerm);
                var doctorsByName = await _doctorDatabaseHelper.GetDoctorsByPartialDoctorName(searchTerm);

                foreach (var doctor in doctorsByDepartment)
                {
                    AvailableDoctors.Add(doctor);
                }

                foreach (var doctor in doctorsByName)
                {
                    var isAlreadyAdded = AvailableDoctors.Any(existingDoctor => existingDoctor.DoctorId == doctor.DoctorId);
                    if (!isAlreadyAdded)
                    {
                        AvailableDoctors.Add(doctor);
                    }
                }

                AvailableDoctors = SortDoctorsByDefaultCriteria(AvailableDoctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
        }

        public List<DoctorModel> GetSearchedDoctors()
        {
            return AvailableDoctors;
        }

        public List<DoctorModel> GetDoctorsSortedBy(SortCriteria sortCriteria)
        {
            return sortCriteria switch
            {
                SortCriteria.RatingHighToLow => AvailableDoctors.OrderByDescending(d => d.Rating).ToList(),
                SortCriteria.RatingLowToHigh => AvailableDoctors.OrderBy(d => d.Rating).ToList(),
                SortCriteria.NameAscending => AvailableDoctors.OrderBy(d => d.DoctorName).ToList(),
                SortCriteria.NameDescending => AvailableDoctors.OrderByDescending(d => d.DoctorName).ToList(),
                SortCriteria.DepartmentAscending => AvailableDoctors.OrderBy(d => d.DepartmentName).ToList(),
                SortCriteria.RatingThenNameThenDepartment => SortDoctorsByDefaultCriteria(AvailableDoctors),
                _ => AvailableDoctors
            };
        }

        private List<DoctorModel> SortDoctorsByDefaultCriteria(List<DoctorModel> doctors)
        {
            return doctors
                .OrderByDescending(d => d.Rating)
                .ThenBy(d => d.DoctorName)
                .ThenBy(d => d.DepartmentName)
                .ToList();
        }
    }

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
