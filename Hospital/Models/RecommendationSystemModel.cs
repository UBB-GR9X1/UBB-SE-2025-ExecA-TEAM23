using Hospital.Managers;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class RecommendationSystemModel
{
    private readonly DoctorService doctorManager;
    private Dictionary<string, Dictionary<int, int>> symptomDepartmentScores;

    public RecommendationSystemModel(DoctorService doctorManager)
    {
        this.doctorManager = doctorManager;
        symptomDepartmentScores = new Dictionary<string, Dictionary<int, int>>();
        InitializeSymptomScores();
    }

    private void InitializeSymptomScores()
    {
        symptomDepartmentScores = new Dictionary<string, Dictionary<int, int>>
        {
            // Symptom Start Scores (less department dominance)
            { "Suddenly", new Dictionary<int, int> { { 1, 3 }, { 2, 3 }, { 3, 2 } } }, // Cardio/Neuro close
            { "After Waking Up", new Dictionary<int, int> { { 2, 0 }, { 5, 3 } } }, // Neuro now leads
            { "After Incident", new Dictionary<int, int> { { 6, 4 }, { 2, 3 } } }, // Ortho/Neuro compete
            { "After Meeting Someone", new Dictionary<int, int> { { 7, 5 }, { 4, 2 } } }, // Derm + Ophthalmology
            { "After Ingestion", new Dictionary<int, int> { { 5, 4 }, { 3, 4 }, { 1, 2 } } }, // Gastro/Peds/Cardio

            // Discomfort Areas (balanced overlaps)
            { "Eyes", new Dictionary<int, int> { { 4, 5 }, { 2, 3 }, { 7, 2 } } }, // Eye + Neuro + Derm
            { "Head", new Dictionary<int, int> { { 2, 5 }, { 1, 4 }, { 3, 3 } } }, // Neuro > Cardio > Peds
            { "Chest", new Dictionary<int, int> { { 1, 6 }, { 2, 0 }, { 5, 2 } } }, // Cardio primary
            { "Stomach", new Dictionary<int, int> { { 5, 5 }, { 3, 4 }, { 2, 2 } } }, // Gastro > Peds > Neuro
            { "Arm", new Dictionary<int, int> { { 6, 5 }, { 7, 3 }, { 2, 2 } } }, // Ortho > Derm > Neuro
            { "Leg", new Dictionary<int, int> { { 6, 5 }, { 7, 3 }, { 1, 2 } } }, // Ortho > Derm > Cardio

            // Symptom Types (cross-department relevance)
            { "Pain", new Dictionary<int, int> { { 1, 4 }, { 6, 4 }, { 2, 0 } } }, // Cardio/Ortho/Neuro
            { "Numbness", new Dictionary<int, int> { { 2, 5 }, { 6, 3 }, { 1, 2 } } }, // Neuro focus
            { "Inflammation", new Dictionary<int, int> { { 7, 4 }, { 5, 4 }, { 4, 2 } } }, // Derm/Gastro
            { "Tenderness", new Dictionary<int, int> { { 6, 4 }, { 5, 3 }, { 2, 2 } } }, // Ortho/Gastro/Neuro
            { "Coloration", new Dictionary<int, int> { { 7, 5 }, { 4, 4 }, { 2, 1 } } } // Derm/Eye/Neuro
        };



    }

    public async Task<DoctorJointModel?> RecommendDoctor(RecommendationSystemFormViewModel formViewModel)
    {
        Dictionary<int, int> departmentScores = new Dictionary<int, int>();

        void AddPoints(string symptom)
        {
            if (string.IsNullOrWhiteSpace(symptom)) return;

            string normalizedSymptom = symptom.Trim();  // Remove leading/trailing spaces

            if (symptomDepartmentScores.ContainsKey(normalizedSymptom))
            {
                foreach (var kvp in symptomDepartmentScores[normalizedSymptom])
                {
                    if (departmentScores.ContainsKey(kvp.Key))
                    {
                        departmentScores[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        departmentScores[kvp.Key] = kvp.Value;
                    }
                }
            }
            else
            {
                Debug.WriteLine($"Warning: '{normalizedSymptom}' not found in dictionary!");
            }
        }

        Debug.WriteLine($"Selected Symptoms: '{formViewModel.SelectedSymptomStart}', '{formViewModel.SelectedDiscomfortArea}', '{formViewModel.SelectedSymptom1}', '{formViewModel.SelectedSymptom2}', '{formViewModel.SelectedSymptom3}'");


        // Accumulate department points based on symptoms
        AddPoints(formViewModel.SelectedSymptomStart);
        AddPoints(formViewModel.SelectedDiscomfortArea);
        AddPoints(formViewModel.SelectedSymptom1);
        AddPoints(formViewModel.SelectedSymptom2);
        AddPoints(formViewModel.SelectedSymptom3);

        Console.WriteLine("Final Department Scores:");
        foreach (var entry in departmentScores)
        {
            Console.WriteLine($"Department {entry.Key}: {entry.Value} points");
        }


        if (departmentScores.Count == 0)
            return null; // No valid department found

        // Select the department with the highest score
        int recommendedDepartment = departmentScores.OrderByDescending(kvp => kvp.Value).First().Key;

        // Get doctors from department
        List<DoctorJointModel> doctors = await doctorManager.GetDoctorsByDepartment(recommendedDepartment);


        Console.WriteLine("Doctors Found:");
        foreach (var doc in doctors)
        {
            Console.WriteLine($"Doctor: {doc.GetDoctorName()}, Department: {doc.GetDoctorDepartment()}");
        }
        // Sorting logic
        DoctorJointModel? recommendedDoctor = doctors
            .OrderByDescending(d => d.GetRegistrationDate())
            .ThenBy(d => d.GetBirthDate()) // Prefer younger doctors
            .ThenBy(d => d.GetDoctorRating()) // Prefer experienced doctors
            .FirstOrDefault();
        //.LastOrDefault();

        // Debug output
        //Console.WriteLine($"Recommended Doctor: {recommendedDoctor?.GetDoctorName() ?? "None"} (Department {recommendedDepartment})");

        return recommendedDoctor;
    }
}