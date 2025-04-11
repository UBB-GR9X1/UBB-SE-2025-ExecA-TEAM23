using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class RecommendationSystemFormViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> SymptomStartOptions { get; }
    public ObservableCollection<string> DiscomfortAreaOptions { get; }
    public ObservableCollection<string> SymptomTypeOptions { get; }

    private const string NoSymptomSelected = "None";

    private string _selectedSymptomStart = string.Empty;
    public string SelectedSymptomStart
    {
        get => _selectedSymptomStart;
        set { _selectedSymptomStart = value; OnPropertyChanged(); }
    }

    private string _selectedDiscomfortArea = string.Empty;
    public string SelectedDiscomfortArea
    {
        get => _selectedDiscomfortArea;
        set { _selectedDiscomfortArea = value; OnPropertyChanged(); }
    }

    private string _selectedSymptomPrimary = NoSymptomSelected;
    public string SelectedSymptomPrimary
    {
        get => _selectedSymptomPrimary;
        set { _selectedSymptomPrimary = value; OnPropertyChanged(); ValidateSymptomSelection(); }
    }

    private string _selectedSymptomSecondary = NoSymptomSelected;
    public string SelectedSymptomSecondary
    {
        get => _selectedSymptomSecondary;
        set { _selectedSymptomSecondary = value; OnPropertyChanged(); ValidateSymptomSelection(); }
    }

    private string _selectedSymptomTertiary = NoSymptomSelected;
    public string SelectedSymptomTertiary
    {
        get => _selectedSymptomTertiary;
        set { _selectedSymptomTertiary = value; OnPropertyChanged(); ValidateSymptomSelection(); }
    }

    public RecommendationSystemFormViewModel()
    {
        SymptomStartOptions = new ObservableCollection<string>
        {
            "Suddenly", "After Waking Up", "After Incident", "After Meeting Someone", "After Ingestion"
        };

        DiscomfortAreaOptions = new ObservableCollection<string>
        {
            "Head", "Eyes", "Neck", "Stomach", "Chest", "Arm", "Leg", "Back", "Shoulder", "Foot"
        };

        SymptomTypeOptions = new ObservableCollection<string>
        {
            "Pain", "Numbness", "Inflammation", "Tenderness", "Coloration", "Itching", "Burning", NoSymptomSelected
        };

        SelectedSymptomPrimary = NoSymptomSelected;
        SelectedSymptomSecondary = NoSymptomSelected;
        SelectedSymptomTertiary = NoSymptomSelected;
    }

    private void ValidateSymptomSelection()
    {
        Debug.WriteLine($"Validating: {SelectedSymptomPrimary}, {SelectedSymptomSecondary}, {SelectedSymptomTertiary}");

        if (SelectedSymptomPrimary != NoSymptomSelected && SelectedSymptomPrimary == SelectedSymptomSecondary)
            SelectedSymptomSecondary = NoSymptomSelected;

        if (SelectedSymptomPrimary != NoSymptomSelected && SelectedSymptomPrimary == SelectedSymptomTertiary)
            SelectedSymptomTertiary = NoSymptomSelected;

        if (SelectedSymptomSecondary != NoSymptomSelected && SelectedSymptomSecondary == SelectedSymptomTertiary)
            SelectedSymptomTertiary = NoSymptomSelected;

        Debug.WriteLine($"After validation: {SelectedSymptomPrimary}, {SelectedSymptomSecondary}, {SelectedSymptomTertiary}");
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
