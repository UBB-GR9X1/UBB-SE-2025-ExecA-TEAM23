// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorViewModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the DoctorViewModel class for handling doctor profile operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Hospital.Managers;
    using Hospital.Models;

    /// <summary>
    /// View model for doctor profile operations. Implements INotifyPropertyChanged for UI binding.
    /// </summary>
    public class DoctorViewModel : INotifyPropertyChanged
    {
        private readonly DoctorManagerModel doctorManagerModel;
        private int userId;
        private string doctorName = string.Empty;
        private int departmentId;
        private string departmentName = string.Empty;
        private double rating;
        private string careerInfo = string.Empty;
        private string avatarUrl = string.Empty;
        private string phoneNumber = string.Empty;
        private string mail = string.Empty;
        private bool isLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorViewModel"/> class.
        /// </summary>
        /// <param name="doctorManagerModel">The doctor manager model.</param>
        /// <param name="userId">The user ID of the doctor.</param>
        public DoctorViewModel(DoctorManagerModel doctorManagerModel, int userId)
        {
            this.doctorManagerModel = doctorManagerModel;
            this.userId = userId;

            // Initialize with default values
            this.DoctorName = "Loading...";
            this.DepartmentName = "Loading department...";
            this.Rating = 0;
            this.CareerInfo = "Loading career information...";
            this.AvatarUrl = "/Assets/default-avatar.png";
            this.PhoneNumber = "Loading phone...";
            this.Mail = "Loading email...";

            // Start async load
            this.OriginalDoctor = DoctorDisplayModel.Default;
            _ = this.LoadDoctorInfoByUserIdAsync(userId);
        }

        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the original doctor model.
        /// </summary>
        public DoctorDisplayModel OriginalDoctor { get; private set; }

        /// <summary>
        /// Gets or sets the user ID of the doctor.
        /// </summary>
        public int UserId
        {
            get => this.userId;
            set
            {
                if (this.userId != value)
                {
                    this.userId = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's name.
        /// </summary>
        public string DoctorName
        {
            get => this.doctorName;
            set
            {
                if (this.doctorName != value)
                {
                    this.doctorName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's department ID.
        /// </summary>
        public int DepartmentId
        {
            get => this.departmentId;
            set
            {
                if (this.departmentId != value)
                {
                    this.departmentId = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the department name.
        /// </summary>
        public string DepartmentName
        {
            get => this.departmentName;
            set
            {
                if (this.departmentName != value)
                {
                    this.departmentName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's rating.
        /// </summary>
        public double Rating
        {
            get => this.rating;
            set
            {
                if (this.rating != value)
                {
                    this.rating = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's career information.
        /// </summary>
        public string CareerInfo
        {
            get => this.careerInfo;
            set
            {
                if (this.careerInfo != value)
                {
                    this.careerInfo = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's avatar URL.
        /// </summary>
        public string AvatarUrl
        {
            get => this.avatarUrl;
            set
            {
                if (this.avatarUrl != value)
                {
                    this.avatarUrl = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's phone number.
        /// </summary>
        public string PhoneNumber
        {
            get => this.phoneNumber;
            set
            {
                if (this.phoneNumber != value)
                {
                    this.phoneNumber = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the doctor's email address.
        /// </summary>
        public string Mail
        {
            get => this.mail;
            set
            {
                if (this.mail != value)
                {
                    this.mail = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view model is loading data.
        /// </summary>
        public bool IsLoading
        {
            get => this.isLoading;
            set
            {
                if (this.isLoading != value)
                {
                    this.isLoading = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads the doctor information based on user ID.
        /// </summary>
        /// <param name="userId">The user ID to load information for.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> LoadDoctorInfoByUserIdAsync(int userId)
        {
            try
            {
                this.IsLoading = true;

                var result = await this.doctorManagerModel.LoadDoctorInfoByUserId(userId);
                if (result && this.doctorManagerModel._doctorInfo != DoctorDisplayModel.Default)
                {
                    var doctor = this.doctorManagerModel._doctorInfo;

                    this.DoctorName = doctor.DoctorName ?? "Not specified";
                    this.DepartmentId = doctor.DepartmentId;
                    this.DepartmentName = doctor.DepartmentName ?? "No department";
                    this.Rating = (double)(doctor.Rating > 0 ? doctor.Rating : 0);
                    this.CareerInfo = doctor.CareerInfo ?? "No career information";
                    this.AvatarUrl = doctor.AvatarUrl ?? "/Assets/default-avatar.png";
                    this.PhoneNumber = doctor.PhoneNumber ?? "Not provided";
                    this.Mail = doctor.Mail ?? "Not provided";

                    this.OriginalDoctor = new DoctorDisplayModel(
                        -1,
                        this.DoctorName,
                        this.DepartmentId,
                        this.DepartmentName,
                        this.Rating,
                        this.CareerInfo,
                        this.AvatarUrl,
                        this.PhoneNumber,
                        this.Mail);

                    return true;
                }

                // Set not found state
                this.DoctorName = "Doctor profile not found";
                this.DepartmentName = "N/A";
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ViewModel: {ex.Message}");

                // Set error state
                this.DoctorName = "Error loading profile";
                this.DepartmentName = "Error";
                this.CareerInfo = "Please try again later";

                return false;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Updates the doctor's name.
        /// </summary>
        /// <param name="name">The new name to set.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> UpdateDoctorName(string name)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this.doctorManagerModel.UpdateDoctorName(this.UserId, name);
                if (result)
                {
                    this.DoctorName = name;
                    this.OriginalDoctor = new DoctorDisplayModel(
                        this.OriginalDoctor.DoctorId,
                        name,
                        this.OriginalDoctor.DepartmentId,
                        this.OriginalDoctor.DepartmentName,
                        this.OriginalDoctor.Rating,
                        this.OriginalDoctor.CareerInfo,
                        this.OriginalDoctor.AvatarUrl,
                        this.OriginalDoctor.PhoneNumber,
                        this.OriginalDoctor.Mail);
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
        /// Updates the doctor's department.
        /// </summary>
        /// <param name="departmentId">The new department ID to set.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> UpdateDepartment(int departmentId)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this.doctorManagerModel.UpdateDepartment(this.UserId, departmentId);
                if (result)
                {
                    this.DepartmentId = departmentId; // Note: You might need to update DepartmentName separately
                    this.OriginalDoctor = new DoctorDisplayModel(
                        this.OriginalDoctor.DoctorId,
                        this.OriginalDoctor.DoctorName,
                        departmentId,
                        this.OriginalDoctor.DepartmentName,
                        this.OriginalDoctor.Rating,
                        this.OriginalDoctor.CareerInfo,
                        this.OriginalDoctor.AvatarUrl,
                        this.OriginalDoctor.PhoneNumber,
                        this.OriginalDoctor.Mail);
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
        /// Updates the doctor's career information.
        /// </summary>
        /// <param name="careerInfo">The new career information to set.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> UpdateCareerInfo(string careerInfo)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this.doctorManagerModel.UpdateCareerInfo(this.UserId, careerInfo);
                if (result)
                {
                    this.CareerInfo = careerInfo;
                    this.OriginalDoctor = new DoctorDisplayModel(
                        this.OriginalDoctor.DoctorId,
                        this.OriginalDoctor.DoctorName,
                        this.OriginalDoctor.DepartmentId,
                        this.OriginalDoctor.DepartmentName,
                        this.OriginalDoctor.Rating,
                        careerInfo,
                        this.OriginalDoctor.AvatarUrl,
                        this.OriginalDoctor.PhoneNumber,
                        this.OriginalDoctor.Mail);
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
        /// Updates the doctor's avatar URL.
        /// </summary>
        /// <param name="avatarUrl">The new avatar URL to set.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> UpdateAvatarUrl(string avatarUrl)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this.doctorManagerModel.UpdateAvatarUrl(this.UserId, avatarUrl);
                if (result)
                {
                    this.AvatarUrl = avatarUrl;
                    this.OriginalDoctor = new DoctorDisplayModel(
                        this.OriginalDoctor.DoctorId,
                        this.OriginalDoctor.DoctorName,
                        this.OriginalDoctor.DepartmentId,
                        this.OriginalDoctor.DepartmentName,
                        this.OriginalDoctor.Rating,
                        this.OriginalDoctor.CareerInfo,
                        this.OriginalDoctor.AvatarUrl,
                        this.OriginalDoctor.PhoneNumber,
                        this.OriginalDoctor.Mail);
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
        /// Updates the doctor's phone number.
        /// </summary>
        /// <param name="phoneNumber">The new phone number to set.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> UpdatePhoneNumber(string phoneNumber)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this.doctorManagerModel.UpdatePhoneNumber(this.UserId, phoneNumber);
                if (result)
                {
                    this.PhoneNumber = phoneNumber;
                    this.OriginalDoctor = new DoctorDisplayModel(
                        this.OriginalDoctor.DoctorId,
                        this.OriginalDoctor.DoctorName,
                        this.OriginalDoctor.DepartmentId,
                        this.OriginalDoctor.DepartmentName,
                        this.OriginalDoctor.Rating,
                        this.OriginalDoctor.CareerInfo,
                        this.OriginalDoctor.AvatarUrl,
                        phoneNumber,
                        this.OriginalDoctor.Mail);
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
        /// Updates the doctor's email address.
        /// </summary>
        /// <param name="mail">The new email address to set.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> UpdateMail(string mail)
        {
            try
            {
                this.IsLoading = true;
                bool result = await this.doctorManagerModel.UpdateEmail(this.UserId, mail);
                if (result)
                {
                    this.Mail = mail;
                    this.OriginalDoctor = new DoctorDisplayModel(
                        this.OriginalDoctor.DoctorId,
                        this.OriginalDoctor.DoctorName,
                        this.OriginalDoctor.DepartmentId,
                        this.OriginalDoctor.DepartmentName,
                        this.OriginalDoctor.Rating,
                        this.OriginalDoctor.CareerInfo,
                        this.OriginalDoctor.AvatarUrl,
                        this.OriginalDoctor.PhoneNumber,
                        mail);
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
        /// Logs a user action.
        /// </summary>
        /// <param name="userId">The user ID performing the action.</param>
        /// <param name="action">The action type to log.</param>
        /// <returns>A task representing the asynchronous operation that returns true if successful.</returns>
        public async Task<bool> LogUpdate(int userId, ActionType action)
        {
            return await this.doctorManagerModel.LogUpdate(userId, action);
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
