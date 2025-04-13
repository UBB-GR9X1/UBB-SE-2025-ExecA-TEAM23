using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

public class RecommendationSystemFormViewModel : INotifyPropertyChanged
{
    private readonly IRecommendationSystem _recommendationSystem;

    public ObservableCollection<string> SymptomStartOptions { get; }
    public ObservableCollection<string> DiscomfortAreaOptions { get; }
    public ObservableCollection<string> SymptomTypeOptions { get; }

    private const string NoSymptomSelected = "None";

    private string _selectedSymptomStart = string.Empty;
    private string _selectedDiscomfortArea = string.Empty;
    private string _selectedSymptomPrimary = NoSymptomSelected;
    private string _selectedSymptomSecondary = NoSymptomSelected;
    private string _selectedSymptomTertiary = NoSymptomSelected;

    private string _doctorName;
    private string _department;
    private string _rating;

    public string SelectedSymptomStart { get => _selectedSymptomStart; set { _selectedSymptomStart = value; OnPropertyChanged(); } }
    public string SelectedDiscomfortArea { get => _selectedDiscomfortArea; set { _selectedDiscomfortArea = value; OnPropertyChanged(); } }
    public string SelectedSymptomPrimary { get => _selectedSymptomPrimary; set { _selectedSymptomPrimary = value; OnPropertyChanged(); ValidateSymptomSelection(); } }
    public string SelectedSymptomSecondary { get => _selectedSymptomSecondary; set { _selectedSymptomSecondary = value; OnPropertyChanged(); ValidateSymptomSelection(); } }
    public string SelectedSymptomTertiary { get => _selectedSymptomTertiary; set { _selectedSymptomTertiary = value; OnPropertyChanged(); ValidateSymptomSelection(); } }

    public string DoctorName { get => _doctorName; set { _doctorName = value; OnPropertyChanged(); } }
    public string Department { get => _department; set { _department = value; OnPropertyChanged(); } }
    public string Rating { get => _rating; set { _rating = value; OnPropertyChanged(); } }

    public ICommand RecommendCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public RecommendationSystemFormViewModel(IRecommendationSystem recommendationSystem)
    {
        _recommendationSystem = recommendationSystem;

        SymptomStartOptions = new ObservableCollection<string> { "Suddenly", "After Waking Up", "After Incident", "After Meeting Someone", "After Ingestion" };
        DiscomfortAreaOptions = new ObservableCollection<string> { "Head", "Eyes", "Neck", "Stomach", "Chest", "Arm", "Leg", "Back", "Shoulder", "Foot" };
        SymptomTypeOptions = new ObservableCollection<string> { "Pain", "Numbness", "Inflammation", "Tenderness", "Coloration", "Itching", "Burning", NoSymptomSelected };

        RecommendCommand = new RelayCommand(async () => await RecommendDoctorAsync());
    }

    private async Task RecommendDoctorAsync()
    {
        var doctor = await _recommendationSystem.RecommendDoctorAsync(this);

        if (doctor != null)
        {
            DoctorName = $"Doctor: {doctor.GetDoctorName()}";
            Department = $"Department: {doctor.GetDoctorDepartment()}";
            Rating = $"Rating: {doctor.GetDoctorRating():0.0}";
        }
        else
        {
            DoctorName = "No suitable doctor found";
            Department = string.Empty;
            Rating = string.Empty;
        }
    }

    private void ValidateSymptomSelection()
    {
        if (SelectedSymptomPrimary != NoSymptomSelected && SelectedSymptomPrimary == SelectedSymptomSecondary)
            SelectedSymptomSecondary = NoSymptomSelected;

        if (SelectedSymptomPrimary != NoSymptomSelected && SelectedSymptomPrimary == SelectedSymptomTertiary)
            SelectedSymptomTertiary = NoSymptomSelected;

        if (SelectedSymptomSecondary != NoSymptomSelected && SelectedSymptomSecondary == SelectedSymptomTertiary)
            SelectedSymptomTertiary = NoSymptomSelected;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
