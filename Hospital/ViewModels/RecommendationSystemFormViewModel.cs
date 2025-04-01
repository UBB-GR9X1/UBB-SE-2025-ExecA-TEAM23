using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        set { _selectedSymptom1 = value; OnPropertyChanged(); ValidateSymptoms(); }
    }

    private string _selectedSymptom2;
    public string SelectedSymptom2
    {
        get => _selectedSymptom2;
        set { _selectedSymptom2 = value; OnPropertyChanged(); ValidateSymptoms(); }
    }

    private string _selectedSymptom3;
    public string SelectedSymptom3
    {
        get => _selectedSymptom3;
        set { _selectedSymptom3 = value; OnPropertyChanged(); ValidateSymptoms(); }
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

        SelectedSymptomStart = SymptomStartOptions[0];
        SelectedDiscomfortArea = SymptomDiscomfortAreas[0];
        SelectedSymptom1 = "None";
        SelectedSymptom2 = "None";
        SelectedSymptom3 = "None";

    }

    private void ValidateSymptoms()
    {
        if (SelectedSymptom1 != "None" && SelectedSymptom2 == SelectedSymptom1)
            SelectedSymptom2 = "None";
        if (SelectedSymptom1 != "None" && SelectedSymptom3 == SelectedSymptom1)
            SelectedSymptom3 = "None";
        if (SelectedSymptom2 != "None" && SelectedSymptom3 == SelectedSymptom2)
            SelectedSymptom3 = "None";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}