using Hospital.Managers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        private readonly PatientManagerModel _patientManagerModel;


        public PatientViewModel(PatientManagerModel patientManagerModel, int userId)
        {
            _patientManagerModel = patientManagerModel;
            _userId = userId;
            LoadPatientInfoByUserIdAsync(userId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _userId;
        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _emergencyContact;
        public string EmergencyContact
        {
            get => _emergencyContact;
            set
            {
                if (_emergencyContact != value)
                {
                    _emergencyContact = value;
                    OnPropertyChanged();
                }
            }
        }

        // Non-editable fields
        private string _bloodType;
        public string BloodType
        {
            get => _bloodType;
            set {
                if (_bloodType != value)
                {
                    _bloodType = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _allergies;
        public string Allergies
        {
            get => _allergies;
            set {
                if (_allergies != value)
                {
                    _allergies = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get => _birthDate;
            set {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cnp;
        public string Cnp
        {
            get => _cnp;
            set {
                if (_cnp != value)
                {
                    _cnp = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set {
                if (_registrationDate != value)
                {
                    _registrationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private float _weight;
        public float Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        // Password fields
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoadPatientInfoByUserIdAsync(int userId)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.LoadPatientInfoByUserId(userId);
                if (result && _patientManagerModel._patientInfo != null)
                {
                    var patient = _patientManagerModel._patientInfo;
                    Name = patient.PatientName;
                    Email = patient.Mail;
                    Password = patient.Password;
                    Username = patient.Username;
                    Address = patient.Address;
                    PhoneNumber = patient.PhoneNumber;
                    EmergencyContact = patient.EmergencyContact;
                    BloodType = patient.BloodType;
                    Allergies = patient.Allergies;
                    BirthDate = patient.BirthDate.ToDateTime(TimeOnly.MinValue);
                    Cnp = patient.Cnp;
                    RegistrationDate = patient.RegistrationDate;
                    Weight = patient.Weight;
                    Height = patient.Height;

                }

                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error loading patient info: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateName(string name)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateName(UserId, name);
                if (result)
                {
                    Name = name;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating name: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateEmail(string email)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateEmail(UserId, email);
                if (result)
                {
                    Email = email;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUsername(string username)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateUsername(UserId, username);
                if (result)
                {
                    Username = username;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating username: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAddress(string address)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateAddress(UserId, address);
                if (result)
                {
                    Address = address;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating address: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdatePassword(string password)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdatePassword(UserId, password);
                if (result)
                {
                    Password = password;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating password: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePhoneNumber(string phoneNumber)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdatePhoneNumber(UserId, phoneNumber);
                if (result)
                {
                    PhoneNumber = phoneNumber;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating phone number: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateEmergencyContact(string emergencyContact)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateEmergencyContact(UserId, emergencyContact);
                if (result)
                {
                    EmergencyContact = emergencyContact;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating emergency contact: {ex.Message}");
                return false;
            }
        }

      

        public async Task<bool> UpdateWeight(float weight)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateWeight(UserId, weight);
                if (result)
                {
                    Weight = weight;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating weight: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateHeight(int height)
        {
            try
            {
                IsLoading = true;
                bool result = await _patientManagerModel.UpdateHeight(UserId, height);
                if (result)
                {
                    Height = height;
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine($"Error updating height: {ex.Message}");
                return false;
            }
        }

    }
}
