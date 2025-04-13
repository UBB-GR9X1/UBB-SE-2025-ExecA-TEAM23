// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatientViewModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the PatientViewModel for managing patient information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Hospital.Managers;
    using Hospital.Models;

    /// <summary>
    /// View model for managing patient information and operations.
    /// </summary>
    public class PatientViewModel : INotifyPropertyChanged
    {
        private readonly PatientManagerModel _patientManagerModel;
        private int _userId;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _username = string.Empty;
        private string _address = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _emergencyContact = string.Empty;
        private string _bloodType = string.Empty;
        private string _allergies = string.Empty;
        private DateTime _birthDate;
        private string _cnp = string.Empty;
        private DateTime _registrationDate;
        private double _weight;
        private int _height;
        private bool _isLoading;
        private string _password = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientViewModel"/> class.
        /// </summary>
        /// <param name="patientManagerModel">The patient manager model.</param>
        /// <param name="userId">The user ID for the patient.</param>
        public PatientViewModel(PatientManagerModel patientManagerModel, int userId)
        {
            this._patientManagerModel = patientManagerModel ?? throw new ArgumentNullException(nameof(patientManagerModel));
            this._userId = userId;
            this.OriginalPatient = PatientJointModel.Default;
            _ = this.LoadPatientInfoByUserIdAsync(userId);
        }

        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the original patient data for comparison.
        /// </summary>
        public PatientJointModel OriginalPatient { get; private set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int UserId
        {
            get => this._userId;
            set
            {
                if (this._userId != value)
                {
                    this._userId = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's name.
        /// </summary>
        public string Name
        {
            get => this._name;
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's email address.
        /// </summary>
        public string Email
        {
            get => this._email;
            set
            {
                if (this._email != value)
                {
                    this._email = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's username.
        /// </summary>
        public string Username
        {
            get => this._username;
            set
            {
                if (this._username != value)
                {
                    this._username = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's address.
        /// </summary>
        public string Address
        {
            get => this._address;
            set
            {
                if (this._address != value)
                {
                    this._address = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's phone number.
        /// </summary>
        public string PhoneNumber
        {
            get => this._phoneNumber;
            set
            {
                if (this._phoneNumber != value)
                {
                    this._phoneNumber = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's emergency contact information.
        /// </summary>
        public string EmergencyContact
        {
            get => this._emergencyContact;
            set
            {
                if (this._emergencyContact != value)
                {
                    this._emergencyContact = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's blood type.
        /// </summary>
        public string BloodType
        {
            get => this._bloodType;
            set
            {
                if (this._bloodType != value)
                {
                    this._bloodType = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's allergies.
        /// </summary>
        public string Allergies
        {
            get => this._allergies;
            set
            {
                if (this._allergies != value)
                {
                    this._allergies = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's birth date.
        /// </summary>
        public DateTime BirthDate
        {
            get => this._birthDate;
            set
            {
                if (this._birthDate != value)
                {
                    this._birthDate = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's CNP (personal numeric code).
        /// </summary>
        public string Cnp
        {
            get => this._cnp;
            set
            {
                if (this._cnp != value)
                {
                    this._cnp = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's registration date.
        /// </summary>
        public DateTime RegistrationDate
        {
            get => this._registrationDate;
            set
            {
                if (this._registrationDate != value)
                {
                    this._registrationDate = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's weight in kilograms.
        /// </summary>
        public double Weight
        {
            get => this._weight;
            set
            {
                if (this._weight != value)
                {
                    this._weight = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's height in centimeters.
        /// </summary>
        public int Height
        {
            get => this._height;
            set
            {
                if (this._height != value)
                {
                    this._height = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether data is currently being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => this._isLoading;
            set
            {
                if (this._isLoading != value)
                {
                    this._isLoading = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the patient's password.
        /// </summary>
        public string Password
        {
            get => this._password;
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads patient information by user ID.
        /// </summary>
        /// <param name="userId">The user ID to load information for.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> LoadPatientInfoByUserIdAsync(int userId)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.LoadPatientInfoByUserId(userId);
                if (result && this._patientManagerModel.PatientInfo != PatientJointModel.Default)
                {
                    var patient = this._patientManagerModel.PatientInfo;
                    this.Name = patient.PatientName;
                    this.Email = patient.Mail;
                    this.Password = patient.Password;
                    this.Username = patient.Username;
                    this.Address = patient.Address;
                    this.PhoneNumber = patient.PhoneNumber;
                    this.EmergencyContact = patient.EmergencyContact;
                    this.BloodType = patient.BloodType;
                    this.Allergies = patient.Allergies;
                    this.BirthDate = patient.BirthDate.ToDateTime(TimeOnly.MinValue);
                    this.Cnp = patient.Cnp;
                    this.RegistrationDate = patient.RegistrationDate;
                    this.Weight = patient.Weight;
                    this.Height = patient.Height;

                    this.OriginalPatient = new PatientJointModel(
                        userId,
                        -1,
                        this.Name,
                        this.BloodType,
                        this.EmergencyContact,
                        this.Allergies,
                        this.Weight,
                        this.Height,
                        this.Username,
                        this.Password,
                        this.Email,
                        patient.BirthDate,
                        this.Cnp,
                        this.Address,
                        this.PhoneNumber,
                        this.RegistrationDate);
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                Console.WriteLine($"Error loading patient info: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates the patient's name.
        /// </summary>
        /// <param name="name">The new name.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateName(string name)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateName(this.UserId, name);
                if (result)
                {
                    this.Name = name;
                    this.OriginalPatient.PatientName = name;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's email.
        /// </summary>
        /// <param name="email">The new email.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateEmail(string email)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateEmail(this.UserId, email);
                if (result)
                {
                    this.Email = email;
                    this.OriginalPatient.Mail = email;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's username.
        /// </summary>
        /// <param name="username">The new username.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateUsername(string username)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateUsername(this.UserId, username);
                if (result)
                {
                    this.Username = username;
                    this.OriginalPatient.Username = username;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's address.
        /// </summary>
        /// <param name="address">The new address.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateAddress(string address)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateAddress(this.UserId, address);
                if (result)
                {
                    this.Address = address;
                    this.OriginalPatient.Address = address;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's password.
        /// </summary>
        /// <param name="password">The new password.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdatePassword(string password)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdatePassword(this.UserId, password);
                if (result)
                {
                    this.Password = password;
                    this.OriginalPatient.Password = password;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's phone number.
        /// </summary>
        /// <param name="phoneNumber">The new phone number.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdatePhoneNumber(string phoneNumber)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdatePhoneNumber(this.UserId, phoneNumber);
                if (result)
                {
                    this.PhoneNumber = phoneNumber;
                    this.OriginalPatient.PhoneNumber = phoneNumber;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's emergency contact.
        /// </summary>
        /// <param name="emergencyContact">The new emergency contact.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateEmergencyContact(string emergencyContact)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateEmergencyContact(this.UserId, emergencyContact);
                if (result)
                {
                    this.EmergencyContact = emergencyContact;
                    this.OriginalPatient.EmergencyContact = emergencyContact;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's weight.
        /// </summary>
        /// <param name="weight">The new weight.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateWeight(double weight)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateWeight(this.UserId, weight);
                if (result)
                {
                    this.Weight = weight;
                    this.OriginalPatient.Weight = weight;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient's height.
        /// </summary>
        /// <param name="height">The new height.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> UpdateHeight(int height)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this._patientManagerModel.UpdateHeight(this.UserId, height);
                if (result)
                {
                    this.Height = height;
                    this.OriginalPatient.Height = height;
                }

                this.IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Logs an update action for the patient.
        /// </summary>
        /// <param name="userId">The user ID to log the action for.</param>
        /// <param name="action">The type of action being logged.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
        public async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await this._patientManagerModel.LogUpdate(userId, action);
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
