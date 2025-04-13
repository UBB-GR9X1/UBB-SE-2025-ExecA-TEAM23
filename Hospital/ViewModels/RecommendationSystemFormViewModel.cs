// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecommendationSystemFormViewModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the RecommendationSystemFormViewModel for handling medical recommendation forms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// View model for managing recommendation system form inputs and validations.
    /// </summary>
    public class RecommendationSystemFormViewModel : INotifyPropertyChanged
    {
        private string selectedSymptomStart = string.Empty;
        private string selectedDiscomfortArea = string.Empty;
        private string selectedSymptom1 = string.Empty;
        private string selectedSymptom2 = string.Empty;
        private string selectedSymptom3 = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationSystemFormViewModel"/> class.
        /// </summary>
        public RecommendationSystemFormViewModel()
        {
            this.SymptomStartOptions = new ObservableCollection<string>
            {
                "Suddenly",
                "After Waking Up",
                "After Incident",
                "After Meeting Someone",
                "After Ingestion",
            };

            this.SymptomDiscomfortAreas = new ObservableCollection<string>
            {
                "Head",
                "Eyes",
                "Neck",
                "Stomach",
                "Chest",
                "Arm",
                "Leg",
                "Back",
                "Shoulder",
                "Foot",
            };

            this.SymptomTypes = new ObservableCollection<string>
            {
                "Pain",
                "Numbness",
                "Inflammation",
                "Tenderness",
                "Coloration",
                "Itching",
                "Burning",
                "None",
            };

            // Default values for the symptoms
            this.SelectedSymptom1 = this.SymptomTypes[7];
            this.SelectedSymptom2 = this.SymptomTypes[7];
            this.SelectedSymptom3 = this.SymptomTypes[7];
        }

        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the collection of symptom start options.
        /// </summary>
        public ObservableCollection<string> SymptomStartOptions { get; set; }

        /// <summary>
        /// Gets or sets the collection of symptom discomfort areas.
        /// </summary>
        public ObservableCollection<string> SymptomDiscomfortAreas { get; set; }

        /// <summary>
        /// Gets or sets the collection of symptom types.
        /// </summary>
        public ObservableCollection<string> SymptomTypes { get; set; }

        /// <summary>
        /// Gets or sets the selected symptom start option.
        /// </summary>
        public string SelectedSymptomStart
        {
            get => this.selectedSymptomStart;
            set
            {
                if (this.selectedSymptomStart != value)
                {
                    this.selectedSymptomStart = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected discomfort area.
        /// </summary>
        public string SelectedDiscomfortArea
        {
            get => this.selectedDiscomfortArea;
            set
            {
                if (this.selectedDiscomfortArea != value)
                {
                    this.selectedDiscomfortArea = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the first selected symptom.
        /// </summary>
        public string SelectedSymptom1
        {
            get => this.selectedSymptom1;
            set
            {
                if (this.selectedSymptom1 != value)
                {
                    this.selectedSymptom1 = value;
                    this.OnPropertyChanged();
                    this.ValidateSymptoms();
                }
            }
        }

        /// <summary>
        /// Gets or sets the second selected symptom.
        /// </summary>
        public string SelectedSymptom2
        {
            get => this.selectedSymptom2;
            set
            {
                if (this.selectedSymptom2 != value)
                {
                    this.selectedSymptom2 = value;
                    this.OnPropertyChanged();
                    this.ValidateSymptoms();
                }
            }
        }

        /// <summary>
        /// Gets or sets the third selected symptom.
        /// </summary>
        public string SelectedSymptom3
        {
            get => this.selectedSymptom3;
            set
            {
                if (this.selectedSymptom3 != value)
                {
                    this.selectedSymptom3 = value;
                    this.OnPropertyChanged();
                    this.ValidateSymptoms();
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Validates symptom selections to ensure there are no duplicates.
        /// </summary>
        private void ValidateSymptoms()
        {
            Debug.WriteLine($"Validating Symptoms: {this.SelectedSymptom1}, {this.SelectedSymptom2}, {this.SelectedSymptom3}");

            // Only perform validation when symptoms are not "None"
            if (this.SelectedSymptom1 != this.SymptomTypes[7] && this.SelectedSymptom2 == this.SelectedSymptom1)
            {
                this.SelectedSymptom2 = string.Empty;
            }

            if (this.SelectedSymptom1 != this.SymptomTypes[7] && this.SelectedSymptom3 == this.SelectedSymptom1)
            {
                this.SelectedSymptom3 = string.Empty;
            }

            if (this.SelectedSymptom2 != this.SymptomTypes[7] && this.SelectedSymptom3 == this.SelectedSymptom2)
            {
                this.SelectedSymptom3 = string.Empty;
            }

            Debug.WriteLine($"After Validation: {this.SelectedSymptom1}, {this.SelectedSymptom2}, {this.SelectedSymptom3}");
        }
    }
}
