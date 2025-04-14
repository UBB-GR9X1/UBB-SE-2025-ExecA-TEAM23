using Hospital.Managers;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public interface    IRecommendationSystem
{
    Task<DoctorJointModel?> RecommendDoctorBasedOnSymptomsAsync(RecommendationSystemFormViewModel vm);
}


public class RecommendationSystemModel : IRecommendationSystem
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

    public RecommendationSystemModel(DoctorService doctorManager)
    {
        _doctorManager = doctorManager;
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

    public async Task<DoctorJointModel?> RecommendDoctorAsync(RecommendationSystemFormViewModel model)
    {
        Dictionary<int, int> departmentScores = new Dictionary<int, int>();

        void AddSymptomScore(string symptom)
        {
            if (string.IsNullOrWhiteSpace(symptom)) return;

            if (_symptomToDepartmentScoreMapping.TryGetValue(symptom.Trim(), out var scores))
            {
                foreach (var kvp in scores)
                {
                    if (!departmentScores.ContainsKey(kvp.Key))
                        departmentScores[kvp.Key] = 0;

                    departmentScores[kvp.Key] += kvp.Value;
                }
            }
        }

        AddSymptomScore(model.SelectedSymptomStart);
        AddSymptomScore(model.SelectedDiscomfortArea);
        AddSymptomScore(model.SelectedSymptomPrimary);
        AddSymptomScore(model.SelectedSymptomSecondary);
        AddSymptomScore(model.SelectedSymptomTertiary);

        if (departmentScores.Count == 0)
            return null;

        int bestDeptId = departmentScores.OrderByDescending(x => x.Value).First().Key;
        var doctors = await _doctorManager.GetDoctorsByDepartment(bestDeptId);

        return doctors
            .OrderByDescending(d => d.GetRegistrationDate())
            .ThenBy(d => d.GetBirthDate())
            .ThenBy(d => d.GetDoctorRating())
            .FirstOrDefault();
    }

    public Task<DoctorJointModel?> RecommendDoctorBasedOnSymptomsAsync(RecommendationSystemFormViewModel vm)
    {
        throw new NotImplementedException();
    }
}