using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

public class RecommendationSystemFormViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> SymptomStartOptions { get; set; }
    public ObservableCollection<string> SymptomDiscomfortAreas { get; set; }
    public ObservableCollection<string> SymptomTypes { get; set; }

    private string _selectedSymptomStart;
    public string SelectedSymptomStart
    {
        get => _selectedSymptomStart;
        set { _selectedSymptomStart = value; OnPropertyChanged(); }
    }

    private string _selectedDiscomfortArea;
    public string SelectedDiscomfortArea
    {
        get => _selectedDiscomfortArea;
        set { _selectedDiscomfortArea = value; OnPropertyChanged(); }
    }

    private string _selectedSymptom1;
    public string SelectedSymptom1
    {
        get => _selectedSymptom1;
        set { _selectedSymptom1 = value; OnPropertyChanged(); }
    }

    private string _selectedSymptom2;
    public string SelectedSymptom2
    {
        get => _selectedSymptom2;
        set { _selectedSymptom2 = value; OnPropertyChanged(); }
    }

    private string _selectedSymptom3;
    public string SelectedSymptom3
    {
        get => _selectedSymptom3;
        set { _selectedSymptom3 = value; OnPropertyChanged(); }
    }



    public RecommendationSystemFormViewModel()
    {

        SymptomStartOptions = new ObservableCollection<string>
        {
            "Suddenly", "After Waking Up", "After Incident", "After Meeting Someone", "After Ingestion"
        };

        SymptomDiscomfortAreas = new ObservableCollection<string>
        {
            "Head", "Eyes", "Neck", "Stomach", "Chest", "Arm", "Leg", "Back", "Shoulder", "Foot"
        };

        SymptomTypes = new ObservableCollection<string>
        {
            "Pain", "Numbness", "Inflammation", "Tenderness", "Coloration", "Itching", "Burning", "None"
        };

        // Here wecan create default values for the fields
        // I opted to use the placeholder text in the XAML file instead since I think it is nicer
        
        //SelectedSymptomStart = SymptomStartOptions[1];
        //SelectedDiscomfortArea = SymptomDiscomfortAreas[4];
        SelectedSymptom1 = SymptomTypes[7];
        SelectedSymptom2 = SymptomTypes[7];
        SelectedSymptom3 = SymptomTypes[7];
        
    }

    private void ValidateSymptoms()
    {
        Debug.WriteLine($"Validating Symptoms: {SelectedSymptom1}, {SelectedSymptom2}, {SelectedSymptom3}");

        // Only perform validation when symptoms are not "None"
        if (SelectedSymptom1 != SymptomTypes[7] && SelectedSymptom2 == SelectedSymptom1)
            SelectedSymptom2 = string.Empty; // Or set to "None" based on your logic
        if (SelectedSymptom1 != SymptomTypes[7] && SelectedSymptom3 == SelectedSymptom1)
            SelectedSymptom3 = string.Empty; // Or set to "None"
        if (SelectedSymptom2 != SymptomTypes[7] && SelectedSymptom3 == SelectedSymptom2)
            SelectedSymptom3 = string.Empty; // Or set to "None"

        Debug.WriteLine($"After Validation: {SelectedSymptom1}, {SelectedSymptom2}, {SelectedSymptom3}");
    }



    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}