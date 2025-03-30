using Hospital.DatabaseServices;
using Hospital.Exceptions;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class DoctorManagerModel
    {
        private readonly DoctorsDatabaseService _doctorDBService;
        private List<DoctorJointModel> doctorList;

        public DoctorManagerModel(DoctorsDatabaseService dbService)
        {
            _doctorDBService = dbService;
            doctorList = new List<DoctorJointModel>();
        }

        public async Task LoadDoctors(int departmentId)
        {
            try
            {
                doctorList = await _doctorDBService.GetDoctorsByDepartment(departmentId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
        }

        public ReadOnlyCollection<DoctorJointModel> GetDoctorsWithRatings()
        {
            return doctorList.AsReadOnly();
        }
    }
}
