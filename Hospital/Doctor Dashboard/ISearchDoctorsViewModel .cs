using Hospital.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public interface ISearchDoctorsViewModel
{
    ObservableCollection<DoctorModel> DoctorList { get; }
    string DepartmentPartialName { get; set; }
    DoctorModel SelectedDoctor { get; set; }
    bool IsProfileOpen { get; set; }

    Task LoadDoctors();
    void ShowDoctorProfile(DoctorModel doctor);
    void CloseDoctorProfile();
}

