using Hospital.DatabaseServices;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    class SearchDoctorsManagerModel
    {
        public List<DoctorDisplayModel> doctorList { get; private set; }
        private DoctorsDatabaseService _doctorDBService;

        public SearchDoctorsManagerModel(DoctorsDatabaseService dbService)
        {
            _doctorDBService = dbService;
            doctorList = new List<DoctorDisplayModel>();
        }

        public async Task LoadDoctors(string departmetnPartialName)
        {
            try
            {
                doctorList.Clear();
                List<DoctorDisplayModel> doctorsList = await _doctorDBService.GetDoctorsByDepartmentPartialName(departmetnPartialName);
                foreach (DoctorDisplayModel doctor in doctorsList)
                {
                    doctorList.Add(doctor);
                }
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
    }
}
