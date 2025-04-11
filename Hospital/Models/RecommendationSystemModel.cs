using Hospital.Managers;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class RecommendationSystemModel
{
    private readonly DoctorManagerModel _doctorManager;
    private Dictionary<string, Dictionary<int, int>> _symptomToDepartmentScoreMapping;

    private const int Cardiology = 1;
    private const int Neurology = 2;
    private const int Pediatrics = 3;
    private const int Ophthalmology = 4;
    private const int Gastroenterology = 5;
    private const int Orthopedics = 6;
    private const int Dermatology = 7;

    public RecommendationSystemModel(DoctorManagerModel doctorManager)
    {
        _doctorManager = doctorManager;
        _symptomToDepartmentScoreMapping = new Dictionary<string, Dictionary<int, int>>();
        InitializeSymptomToDepartmentScores();
    }

    private void InitializeSymptomToDepartmentScores()
    {
        _symptomToDepartmentScoreMapping = new Dictionary<string, Dictionary<int, int>>
        {
            { "Suddenly", new Dictionary<int, int> { { Cardiology, 3 }, { Neurology, 3 }, { Pediatrics, 2 } } },
            { "After Waking Up", new Dictionary<int, int> { { Neurology, 0 }, { Gastroenterology, 3 } } },
            { "After Incident", new Dictionary<int, int> { { Orthopedics, 4 }, { Neurology, 3 } } },
            { "After Meeting Someone", new Dictionary<int, int> { { Dermatology, 5 }, { Ophthalmology, 2 } } },
            { "After Ingestion", new Dictionary<int, int> { { Gastroenterology, 4 }, { Pediatrics, 4 }, { Cardiology, 2 } } },

            { "Eyes", new Dictionary<int, int> { { Ophthalmology, 5 }, { Neurology, 3 }, { Dermatology, 2 } } },
            { "Head", new Dictionary<int, int> { { Neurology, 5 }, { Cardiology, 4 }, { Pediatrics, 3 } } },
            { "Chest", new Dictionary<int, int> { { Cardiology, 6 }, { Neurology, 0 }, { Gastroenterology, 2 } } },
            { "Stomach", new Dictionary<int, int> { { Gastroenterology, 5 }, { Pediatrics, 4 }, { Neurology, 2 } } },
            { "Arm", new Dictionary<int, int> { { Orthopedics, 5 }, { Dermatology, 3 }, { Neurology, 2 } } },
            { "Leg", new Dictionary<int, int> { { Orthopedics, 5 }, { Dermatology, 3 }, { Cardiology, 2 } } },

            { "Pain", new Dictionary<int, int> { { Cardiology, 4 }, { Orthopedics, 4 }, { Neurology, 0 } } },
            { "Numbness", new Dictionary<int, int> { { Neurology, 5 }, { Orthopedics, 3 }, { Cardiology, 2 } } },
            { "Inflammation", new Dictionary<int, int> { { Dermatology, 4 }, { Gastroenterology, 4 }, { Ophthalmology, 2 } } },
            { "Tenderness", new Dictionary<int, int> { { Orthopedics, 4 }, { Gastroenterology, 3 }, { Neurology, 2 } } },
            { "Coloration", new Dictionary<int, int> { { Dermatology, 5 }, { Ophthalmology, 4 }, { Neurology, 1 } } }
        };
    }

    public async Task<DoctorJointModel?> RecommendDoctorBasedOnSymptomsAsync(RecommendationSystemFormViewModel symptomFormViewModel)
    {
        Dictionary<int, int> departmentScores = new Dictionary<int, int>();

        void AddSymptomScoreToDepartments(string symptom)
        {
            if (string.IsNullOrWhiteSpace(symptom)) return;

            string cleanedSymptom = symptom.Trim();

            if (_symptomToDepartmentScoreMapping.TryGetValue(cleanedSymptom, out var departmentScoreMap))
            {
                foreach (var departmentScore in departmentScoreMap)
                {
                    int departmentId = departmentScore.Key;
                    int points = departmentScore.Value;

                    if (departmentScores.ContainsKey(departmentId))
                    {
                        departmentScores[departmentId] += points;
                    }
                    else
                    {
                        departmentScores[departmentId] = points;
                    }
                }
            }
            else
            {
                Debug.WriteLine($"[Warning] Symptom '{cleanedSymptom}' was not recognized.");
            }
        }

        Debug.WriteLine($"Selected Symptoms: '{symptomFormViewModel.SelectedSymptomStart}', '{symptomFormViewModel.SelectedDiscomfortArea}', '{symptomFormViewModel.SelectedSymptomPrimary}', '{symptomFormViewModel.SelectedSymptomSecondary}', '{symptomFormViewModel.SelectedSymptomTertiary}'");

        AddSymptomScoreToDepartments(symptomFormViewModel.SelectedSymptomStart);
        AddSymptomScoreToDepartments(symptomFormViewModel.SelectedDiscomfortArea);
        AddSymptomScoreToDepartments(symptomFormViewModel.SelectedSymptomPrimary);
        AddSymptomScoreToDepartments(symptomFormViewModel.SelectedSymptomSecondary);
        AddSymptomScoreToDepartments(symptomFormViewModel.SelectedSymptomTertiary);

        if (departmentScores.Count == 0)
            return null;

        int mostRelevantDepartmentId = departmentScores
            .OrderByDescending(entry => entry.Value)
            .First().Key;

        List<DoctorJointModel> doctorsInDepartment = await _doctorManager.GetDoctorsByDepartment(mostRelevantDepartmentId);

        DoctorJointModel? selectedDoctor = doctorsInDepartment
            .OrderByDescending(doctor => doctor.GetRegistrationDate())
            .ThenBy(doctor => doctor.GetBirthDate())
            .ThenBy(doctor => doctor.GetDoctorRating())
            .FirstOrDefault();

        return selectedDoctor;
    }
}