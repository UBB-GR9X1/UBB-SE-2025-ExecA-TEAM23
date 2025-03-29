using Hospital.Managers;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class SearchDoctorsViewModel
    {
        private readonly SearchDoctorsManagerModel _searchDoctorsManager;
        private readonly string _departmentPartialName;

        public List<DoctorDisplayModel> DoctorList { get; private set; }

        public SearchDoctorsViewModel(SearchDoctorsManagerModel searchDoctorsManager, string departmentPartialName)
        {
            _searchDoctorsManager = searchDoctorsManager;
            _departmentPartialName = departmentPartialName;
            DoctorList = new List<DoctorDisplayModel>();
        }

        public async Task LoadDoctors()
        {
            try
            {
                await _searchDoctorsManager.LoadDoctors(_departmentPartialName);
                DoctorList = _searchDoctorsManager.GetSearchedDoctors();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
        }
}
