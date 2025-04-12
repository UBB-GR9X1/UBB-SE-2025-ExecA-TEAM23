using Hospital.Managers;
using Hospital.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class SearchDoctorsViewModel : INotifyPropertyChanged
    {
        public readonly SearchDoctorsManagerModel _searchDoctorsManager;
        private string _departmentPartialName;
        private DoctorDisplayModel _selectedDoctor = DoctorDisplayModel.Default;
        private bool _isProfileOpen;

        private ObservableCollection<DoctorDisplayModel> _doctorList;
        public ObservableCollection<DoctorDisplayModel> DoctorList
        {
            get => _doctorList;
            private set
            {
                _doctorList = value;
                OnPropertyChanged();
            }
        }

        public string DepartmentPartialName
        {
            get => _departmentPartialName;
            set
            {
                _departmentPartialName = value;
                OnPropertyChanged();
            }
        }

        public DoctorDisplayModel SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged();
            }
        }

        public bool IsProfileOpen
        {
            get => _isProfileOpen;
            set
            {
                _isProfileOpen = value;
                OnPropertyChanged();
            }
        }

        public SearchDoctorsViewModel(SearchDoctorsManagerModel searchDoctorsManager, string departmentPartialName)
        {
            _searchDoctorsManager = searchDoctorsManager;
            _departmentPartialName = departmentPartialName;
            _doctorList = new ObservableCollection<DoctorDisplayModel>();
            _isProfileOpen = false;
        }

        public async Task LoadDoctors()
        {
            try
            {
                await _searchDoctorsManager.LoadDoctors(_departmentPartialName);

                // Clear and repopulate the collection with new results
                DoctorList.Clear();
                foreach (var doctor in _searchDoctorsManager.GetSearchedDoctors())
                {
                    DoctorList.Add(doctor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
        }

        // Methods to handle the doctor profile
        public void ShowDoctorProfile(DoctorDisplayModel doctor)
        {
            SelectedDoctor = doctor;
            IsProfileOpen = true;
        }

        public void CloseDoctorProfile()
        {
            IsProfileOpen = false;
            SelectedDoctor = DoctorDisplayModel.Default;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
