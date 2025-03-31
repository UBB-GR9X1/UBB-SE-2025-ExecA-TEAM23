using Hospital.Models;
using Hospital.Managers; // Use DoctorManagerModel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.DatabaseServices;

public class RecommendationSystemModel
{
    private readonly DoctorManagerModel _doctorManager;
    private Dictionary<int, int> _symptomPoints;

    public RecommendationSystemModel(DoctorManagerModel doctorManager)
    {
        _doctorManager = doctorManager;
        InitializeSymptomPoints();
    }

    private void InitializeSymptomPoints()
    {
        _symptomPoints = new Dictionary<int, int>
        {
            { 1, 10 }, { 2, 8 }, { 3, 6 }, { 4, 4 }, { 5, 2 }, // Higher = prefers younger doctor
            { 6, -5 }, { 7, -10 }  // Lower = prefers older doctor
        };
    }

    public async Task<DoctorJointModel?> RecommendDoctor(RecommendationSystemFormViewModel formViewModel)
    {
        // Aggregate symptom score
        int totalPoints =
            _symptomPoints.GetValueOrDefault(formViewModel.SelectedSymptom1, 0) +
            _symptomPoints.GetValueOrDefault(formViewModel.SelectedSymptom2, 0) +
            _symptomPoints.GetValueOrDefault(formViewModel.SelectedSymptom3, 0) +
            _symptomPoints.GetValueOrDefault(formViewModel.SelectedSymptom4, 0) +
            _symptomPoints.GetValueOrDefault(formViewModel.SelectedSymptom5, 0);

        // Determine department based on symptoms
        int departmentId = DetermineDepartment(formViewModel);

        // Get doctors from department
        List<DoctorJointModel> doctors = await _doctorManager.GetDoctorsByDepartment(departmentId);

        // Sorting logic
        return doctors
            .OrderByDescending(d => d.DoctorRating) // Prefer higher-rated doctors
            .ThenBy(d => totalPoints >= 0 ? d.BirthDate : DateTime.MaxValue) // Prefer younger if totalPoints >= 0
            .ThenBy(d => totalPoints < 0 ? d.RegistrationDate : DateTime.MaxValue) // Prefer experienced if totalPoints < 0
            .FirstOrDefault(); // Get best match
    }

    private int DetermineDepartment(RecommendationSystemFormViewModel formViewModel)
    {
        // Placeholder logic (replace with real rules)
        return 1;
    }
}
