using Hospital.Configs;
using Hospital.DatabaseServices;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Managers
{
    public class DoctorManagerModel
    {
        private readonly DoctorsDatabaseService _doctorDBService;

        public DoctorManagerModel(DoctorsDatabaseService doctorDBService)
        {
            _doctorDBService = doctorDBService ?? throw new ArgumentNullException(nameof(doctorDBService));
        }

        private readonly Config _config;
        public DoctorDisplayModel _doctorInfo { get; private set; }
        public List<DoctorDisplayModel> _doctorList { get; private set; }


        // This method will be used to get the doctors from the database
        public async Task<List<DoctorJointModel>> GetDoctorsByDepartment(int departmentId)
        {
            const string querySelectDepartments = @"SELECT
                d.DoctorId,
                d.UserId,
                u.Username,
                d.DepartmentId,
                d.DoctorRating, 
                d.LicenseNumber,
                u.Password,
                u.Mail,
                u.BirthDate,
                u.Cnp,
                u.Address,
                u.PhoneNumber,
                u.RegistrationDate
                FROM Doctors d
                INNER JOIN Users u
                ON d.UserId = u.UserId
                WHERE DepartmentId = @departmentId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                //Prepare the command
                SqlCommand selectCommand = new SqlCommand(querySelectDepartments, connection);

                //Insert parameters
                selectCommand.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);


                //Prepare the list of doctors
                List<DoctorJointModel> doctorList = new List<DoctorJointModel>();

                //Read the data from the database
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int doctorId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    string username = reader.GetString(2);
                    int depId = reader.GetInt32(3);
                    double rating = reader.GetDouble(4);
                    string licenseNumber = reader.GetString(5);
                    string password = reader.GetString(6);
                    string mail = reader.GetString(7);
                    DateOnly birthDate = reader.GetFieldValue<DateOnly>(8);
                    string cnp = reader.GetString(9);
                    string address = reader.GetString(10);
                    string phoneNumber = reader.GetString(11);
                    DateTime registrationDate = reader.GetDateTime(12);

                    DoctorJointModel doctor = new DoctorJointModel(
                        doctorId, userId, username, depId, rating, licenseNumber,
                        username, password, mail, birthDate, cnp, address, phoneNumber, registrationDate
                    );

                    doctorList.Add(doctor);
                }
                return doctorList;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<DoctorJointModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<DoctorJointModel>();
            }
        }
        public async Task<List<DoctorJointModel>> GetAllDoctors()
        {
            const string querySelectAllDoctors = @"SELECT
                d.DoctorId,
                d.UserId,
                u.Username,
                d.DepartmentId,
                d.DoctorRating, 
                d.LicenseNumber,
                u.Password,
                u.Mail,
                u.BirthDate,
                u.Cnp,
                u.Address,
                u.PhoneNumber,
                u.RegistrationDate
                FROM Doctors d
                INNER JOIN Users u
                ON d.UserId = u.UserId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(querySelectAllDoctors, connection);
                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);

                List<DoctorJointModel> doctorList = new List<DoctorJointModel>();

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int doctorId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    string username = reader.GetString(2);
                    int depId = reader.GetInt32(3);
                    double rating = reader.GetDouble(4);
                    string licenseNumber = reader.GetString(5);
                    string password = reader.GetString(6);
                    string mail = reader.GetString(7);
                    DateOnly birthDate = reader.GetFieldValue<DateOnly>(8);
                    string cnp = reader.GetString(9);
                    string address = reader.GetString(10);
                    string phoneNumber = reader.GetString(11);
                    DateTime registrationDate = reader.GetDateTime(12);

                    DoctorJointModel doctor = new DoctorJointModel(
                        doctorId, userId, username, depId, rating, licenseNumber,
                        username, password, mail, birthDate, cnp, address, phoneNumber, registrationDate
                    );

                    doctorList.Add(doctor);
                }
                return doctorList;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return new List<DoctorJointModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return new List<DoctorJointModel>();
            }
        }

        public async Task<bool> LoadDoctorInfoByUserId(int doctorId)
        {
            try
            {
                _doctorInfo = await _doctorDBService.GetDoctorById(doctorId);

                if (_doctorInfo != null)
                {
                    Debug.WriteLine($"Successfully loaded doctor: {_doctorInfo.DoctorName}");
                    return true;
                }
                Debug.WriteLine($"No doctor found for user ID: {doctorId}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading doctor info: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SearchDoctorsByDepartment(string departmentPartialName)
        {
            _doctorList = await _doctorDBService.GetDoctorsByDepartmentPartialName(departmentPartialName);
            return _doctorList != null;
        }

        public async Task<bool> SearchDoctorsByName(string namePartial)
        {
            _doctorList = await _doctorDBService.GetDoctorsByPartialDoctorName(namePartial);
            return _doctorList != null;
        }

        public async Task<bool> UpdateDoctorName(int doctorId, string name)
        {
            return await _doctorDBService.UpdateDoctorName(doctorId, name);
        }

        public async Task<bool> UpdateDepartment(int doctorId, int departmentId)
        {
            return await _doctorDBService.UpdateDoctorDepartment(doctorId, departmentId);
        }

        public async Task<bool> UpdateRating(int doctorId, double rating)
        {
            return await _doctorDBService.UpdateDoctorRating(doctorId, rating);
        }

        public async Task<bool> UpdateCareerInfo(int doctorId, string careerInfo)
        {
            return await _doctorDBService.UpdateDoctorCareerInfo(doctorId, careerInfo);
        }

        public async Task<bool> UpdateAvatarUrl(int doctorId, string avatarUrl)
        {
            return await _doctorDBService.UpdateDoctorAvatarUrl(doctorId, avatarUrl);
        }

        public async Task<bool> UpdatePhoneNumber(int doctorId, string phoneNumber)
        {
            return await _doctorDBService.UpdateDoctorPhoneNumber(doctorId, phoneNumber);
        }

        public async Task<bool> UpdateEmail(int doctorId, string email)
        {
            return await _doctorDBService.UpdateDoctorEmail(doctorId, email);
        }

        public async Task<bool> UpdateDoctorInfo(DoctorDisplayModel doctor)
        {
            return await _doctorDBService.UpdateDoctorInfo(doctor);
        }
    }
}