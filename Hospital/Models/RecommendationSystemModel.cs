using Hospital.Models;
using Hospital.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.DatabaseServices;

public class RecommendationSystemModel
{
    private readonly DoctorManagerModel _doctorManager;
    private Dictionary<string, Dictionary<int, int>> _symptomDepartmentScores;

    public RecommendationSystemModel(DoctorManagerModel doctorManager)
    {
        _doctorManager = doctorManager;
        InitializeSymptomScores();
    }

    private void InitializeSymptomScores()
    {
        _symptomDepartmentScores = new Dictionary<string, Dictionary<int, int>>
        {
            // SymptomStart Scores
            { "suddenly", new Dictionary<int, int> { { 1, 5 }, { 2, 3 }, { 3, 2 } } }, // Cardiology, Neurology, Pediatrics
            { "after waking up", new Dictionary<int, int> { { 2, 4 }, { 5, 3 } } }, // Neurology, General Practice
            { "after incident", new Dictionary<int, int> { { 6, 6 }, { 2, 4 } } }, // Orthopedics, Neurology
            { "after meeting someone", new Dictionary<int, int> { { 7, 5 } } }, // Psychiatry
            { "after ingestion", new Dictionary<int, int> { { 3, 6 }, { 8, 4 } } }, // Pediatrics, Gastroenterology

            // Discomfort Area Scores
            { "eyes", new Dictionary<int, int> { { 9, 5 }, { 2, 4 } } }, // Ophthalmology, Neurology
            { "head", new Dictionary<int, int> { { 2, 6 }, { 7, 4 } } }, // Neurology, Psychiatry
            { "chest", new Dictionary<int, int> { { 1, 7 } } }, // Cardiology
            { "stomach", new Dictionary<int, int> { { 3, 5 }, { 8, 4 } } }, // Pediatrics, Gastroenterology
            { "arm", new Dictionary<int, int> { { 6, 5 } } }, // Orthopedics
            { "leg", new Dictionary<int, int> { { 6, 5 } } }, // Orthopedics

            // Symptom Type Scores
            { "pain", new Dictionary<int, int> { { 1, 4 }, { 6, 6 } } }, // Cardiology, Orthopedics
            { "numbness", new Dictionary<int, int> { { 2, 6 }, { 6, 4 } } }, // Neurology, Orthopedics
            { "inflammation", new Dictionary<int, int> { { 8, 5 }, { 3, 3 } } }, // Gastroenterology, Pediatrics
            { "tenderness", new Dictionary<int, int> { { 6, 5 }, { 8, 3 } } }, // Orthopedics, Gastroenterology
            { "coloration", new Dictionary<int, int> { { 9, 4 }, { 3, 3 } } }, // Ophthalmology, Pediatrics
        };
    }

    public async Task<DoctorJointModel?> RecommendDoctor(RecommendationSystemFormViewModel formViewModel)
    {
        Dictionary<int, int> departmentScores = new();

        void AddPoints(string symptom)
        {
            if (string.IsNullOrWhiteSpace(symptom)) return;
            symptom = symptom.ToLower();
            if (_symptomDepartmentScores.ContainsKey(symptom))
            {
                foreach (var kvp in _symptomDepartmentScores[symptom])
                {
                    if (departmentScores.ContainsKey(kvp.Key))
                        departmentScores[kvp.Key] += kvp.Value;
                    else
                        departmentScores[kvp.Key] = kvp.Value;
                }
            }
        }

        // Accumulate department points based on symptoms
        AddPoints(formViewModel.SelectedSymptomStart);
        AddPoints(formViewModel.SelectedDiscomfortArea);
        AddPoints(formViewModel.SelectedSymptom1);
        AddPoints(formViewModel.SelectedSymptom2);
        AddPoints(formViewModel.SelectedSymptom3);

        if (departmentScores.Count == 0)
            return null; // No valid department found

        // Select the department with the highest score
        int recommendedDepartment = departmentScores.OrderByDescending(kvp => kvp.Value).First().Key;

        // Get doctors from department
        List<DoctorJointModel> doctors = await _doctorManager.GetDoctorsByDepartment(recommendedDepartment);

        // Sorting logic
        DoctorJointModel? recommendedDoctor = doctors
            .OrderByDescending(d => d.GetDoctorRating())
            .ThenBy(d => d.GetBirthDate()) // Prefer younger doctors
            .ThenBy(d => d.GetRegistrationDate()) // Prefer experienced doctors
            .FirstOrDefault();

        // Debug output
        //Console.WriteLine($"Recommended Doctor: {recommendedDoctor?.GetDoctorName() ?? "None"} (Department {recommendedDepartment})");

        return recommendedDoctor;
    }
}