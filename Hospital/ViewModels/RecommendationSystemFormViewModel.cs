using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class RecommendationSystemFormViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<int> SymptomOptions { get; set; }

    private int _selectedSymptom1;
    public int SelectedSymptom1
    {
        get => _selectedSymptom1;
        set { _selectedSymptom1 = value; OnPropertyChanged(); }
    }

    private int _selectedSymptom2;
    public int SelectedSymptom2
    {
        get => _selectedSymptom2;
        set { _selectedSymptom2 = value; OnPropertyChanged(); }
    }

    private int _selectedSymptom3;
    public int SelectedSymptom3
    {
        get => _selectedSymptom3;
        set { _selectedSymptom3 = value; OnPropertyChanged(); }
    }

    private int _selectedSymptom4;
    public int SelectedSymptom4
    {
        get => _selectedSymptom4;
        set { _selectedSymptom4 = value; OnPropertyChanged(); }
    }

    private int _selectedSymptom5;
    public int SelectedSymptom5
    {
        get => _selectedSymptom5;
        set { _selectedSymptom5 = value; OnPropertyChanged(); }
    }

    public RecommendationSystemFormViewModel()
    {
        SymptomOptions = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7 };
        SelectedSymptom1 = -1;
        SelectedSymptom2 = -1;
        SelectedSymptom3 = -1;
        SelectedSymptom4 = -1;
        SelectedSymptom5 = -1;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
