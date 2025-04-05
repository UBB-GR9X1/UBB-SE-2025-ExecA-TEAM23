using Hospital.Managers;
using Hospital.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class DoctorViewModel : INotifyPropertyChanged
    {
        private readonly DoctorManagerModel _doctorManagerModel;
        
        public DoctorViewModel(DoctorManagerModel doctorManagerModel, int userId)
        {
            _doctorManagerModel = doctorManagerModel;
            _userId = userId;

            // Initialize with default values
            DoctorName = "Loading...";
            DepartmentName = "Loading department...";
            Rating = 0;
            CareerInfo = "Loading career information...";
            AvatarUrl = "/Assets/default-avatar.png";
            PhoneNumber = "Loading phone...";
            Mail = "Loading email...";

            // Start async load
            _ = LoadDoctorInfoByUserIdAsync(userId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DoctorDisplayModel _originalDoctor { get; private set; }

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

        private string _doctorName;
        public string DoctorName
        {
            get => _doctorName;
            set
            {
                if (_doctorName != value)
                {
                    _doctorName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _departmentId;
        public int DepartmentId
        {
            get => _departmentId;
            set
            {
                if (_departmentId != value)
                {
                    _departmentId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                if (_departmentName != value)
                {
                    _departmentName = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _rating;
        public double Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _careerInfo;
        public string CareerInfo
        {
            get => _careerInfo;
            set
            {
                if (_careerInfo != value)
                {
                    _careerInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _avatarUrl;
        public string AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = value;
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

        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                if (_mail != value)
                {
                    _mail = value;
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

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoadDoctorInfoByUserIdAsync(int userId)
        {
            try
            {
                IsLoading = true;

                var result = await _doctorManagerModel.LoadDoctorInfoByUserId(userId);
                if (result && _doctorManagerModel._doctorInfo != DoctorDisplayModel.Default)
                {
                    var doctor = _doctorManagerModel._doctorInfo;

                    DoctorName = doctor.DoctorName ?? "Not specified";
                    DepartmentId = doctor.DepartmentId;
                    DepartmentName = doctor.DepartmentName ?? "No department";
                    Rating = (double)(doctor.Rating > 0 ? doctor.Rating : 0);
                    CareerInfo = doctor.CareerInfo ?? "No career information";
                    AvatarUrl = doctor.AvatarUrl ?? "/Assets/default-avatar.png";
                    PhoneNumber = doctor.PhoneNumber ?? "Not provided";
                    Mail = doctor.Mail ?? "Not provided";

                    _originalDoctor = new DoctorDisplayModel(-1, DoctorName, DepartmentId, DepartmentName, Rating, CareerInfo, AvatarUrl,
                        PhoneNumber, Mail);

                    return true;
                }

                // Set not found state
                DoctorName = "Doctor profile not found";
                DepartmentName = "N/A";
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ViewModel: {ex.Message}");

                // Set error state
                DoctorName = "Error loading profile";
                DepartmentName = "Error";
                CareerInfo = "Please try again later";

                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> UpdateDoctorName(string name)
        {
            try
            {
                IsLoading = true;
                bool result = await _doctorManagerModel.UpdateDoctorName(UserId, name);
                if (result)
                {
                    DoctorName = name;
                    _originalDoctor = new DoctorDisplayModel(_originalDoctor.DoctorId, name, _originalDoctor.DepartmentId,
    _originalDoctor.DepartmentName, _originalDoctor.Rating, _originalDoctor.CareerInfo, _originalDoctor.AvatarUrl,
    _originalDoctor.PhoneNumber, _originalDoctor.Mail);
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateDepartment(int departmentId)
        {
            try
            {
                IsLoading = true;
                bool result = await _doctorManagerModel.UpdateDepartment(UserId, departmentId);
                if (result)
                {
                    DepartmentId = departmentId;
                    // Note: You might need to update DepartmentName separately
                    _originalDoctor = new DoctorDisplayModel(_originalDoctor.DoctorId, _originalDoctor.DoctorName, departmentId,
    _originalDoctor.DepartmentName, _originalDoctor.Rating, _originalDoctor.CareerInfo, _originalDoctor.AvatarUrl,
    _originalDoctor.PhoneNumber, _originalDoctor.Mail);
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCareerInfo(string careerInfo)
        {
            try
            {
                IsLoading = true;
                bool result = await _doctorManagerModel.UpdateCareerInfo(UserId, careerInfo);
                if (result)
                {
                    CareerInfo = careerInfo;
                    _originalDoctor = new DoctorDisplayModel(_originalDoctor.DoctorId, _originalDoctor.DoctorName, _originalDoctor.DepartmentId,
    _originalDoctor.DepartmentName, _originalDoctor.Rating, careerInfo, _originalDoctor.AvatarUrl,
    _originalDoctor.PhoneNumber, _originalDoctor.Mail);
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAvatarUrl(string avatarUrl)
        {
            try
            {
                IsLoading = true;
                bool result = await _doctorManagerModel.UpdateAvatarUrl(UserId, avatarUrl);
                if (result)
                {
                    AvatarUrl = avatarUrl;
                    _originalDoctor = new DoctorDisplayModel(_originalDoctor.DoctorId, _originalDoctor.DoctorName, _originalDoctor.DepartmentId,
    _originalDoctor.DepartmentName, _originalDoctor.Rating, _originalDoctor.CareerInfo, _originalDoctor.AvatarUrl,
    _originalDoctor.PhoneNumber, _originalDoctor.Mail);
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdatePhoneNumber(string phoneNumber)
        {
            try
            {
                IsLoading = true;
                bool result = await _doctorManagerModel.UpdatePhoneNumber(UserId, phoneNumber);
                if (result)
                {
                    PhoneNumber = phoneNumber;
                    _originalDoctor = new DoctorDisplayModel(_originalDoctor.DoctorId, _originalDoctor.DoctorName, _originalDoctor.DepartmentId,
                        _originalDoctor.DepartmentName, _originalDoctor.Rating, _originalDoctor.CareerInfo, _originalDoctor.AvatarUrl,
                        phoneNumber, _originalDoctor.Mail);
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateMail(string mail)
        {
            try
            {
                IsLoading = true;
                bool result = await _doctorManagerModel.UpdateEmail(UserId, mail);
                if (result)
                {
                    Mail = mail;
                    _originalDoctor = new DoctorDisplayModel(_originalDoctor.DoctorId, _originalDoctor.DoctorName, _originalDoctor.DepartmentId,
                    _originalDoctor.DepartmentName, _originalDoctor.Rating, _originalDoctor.CareerInfo, _originalDoctor.AvatarUrl,
                    _originalDoctor.PhoneNumber, mail);
                }
                IsLoading = false;
                return result;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                throw new Exception(ex.Message);
            }
        }
    }
}