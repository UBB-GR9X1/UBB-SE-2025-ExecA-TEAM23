using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Repositories;

namespace Hospital.Services
{
    public class SearchDoctorsService : ISearchDoctorsService
    {
        private readonly IDoctorRepository _doctorRepository;
        public List<DoctorModel> AvailableDoctors { get; private set; }

        public SearchDoctorsService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            AvailableDoctors = new List<DoctorModel>();
        }

        public async Task LoadDoctors(string searchTerm)
        {
            try
            {
                AvailableDoctors.Clear();
                var doctorsByDepartment = await _doctorRepository.GetDoctorsByDepartmentPartialName(searchTerm);
                var doctorsByName = await _doctorRepository.GetDoctorsByPartialDoctorName(searchTerm);

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
            catch (Exception error)
            {
                Console.WriteLine($"Error loading doctors: {error.Message}");
            }
        }

        public List<DoctorModel> GetSearchedDoctors()
        {
            return AvailableDoctors;
        }

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

        private List<DoctorModel> SortDoctorsByDefaultCriteria(List<DoctorModel> doctors)
        {
            return doctors
                .OrderByDescending(doctor => doctor.Rating)
                .ThenBy(doctor => doctor.DoctorName)
                .ThenBy(doctor => doctor.DepartmentName)
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