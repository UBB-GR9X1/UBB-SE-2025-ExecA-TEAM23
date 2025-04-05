using Hospital.Configs;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.DatabaseServices
{
    public class PatientsDatabaseService
    {
        private readonly Config _config;
        public PatientsDatabaseService()
        {
            _config = Config.GetInstance();
        }

        public async Task<List<PatientJointModel>> GetAllPatients()
        {
            const string querySelectPatients = @"SELECT 
                    p.PatientId,
                    p.UserId,
                    u.Username,
                    u.Name,
                    u.BirthDate,
                    u.Mail,
                    u.PhoneNumber,
                    u.Address,
                    u.Cnp,
                    u.RegistrationDate,
                    u.AvatarUrl,
                    p.BloodType,
                    p.EmergencyContact,
                    p.Allergies,
                    p.Weight,
                    p.Height,
                    u.Password
                FROM Patients p
                INNER JOIN Users u ON p.UserId = u.UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand selectCommand = new SqlCommand(querySelectPatients, connection);
                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                List<PatientJointModel> patientList = new List<PatientJointModel>();
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int patientId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    string username = reader.GetString(2);
                    string name = reader.GetString(3);
                    DateTime birthDateTime = reader.GetDateTime(4);
                    DateOnly birthDate = DateOnly.FromDateTime(birthDateTime);
                    string mail = reader.GetString(5);
                    string phoneNumber = reader.GetString(6);
                    string address = reader.GetString(7);
                    string cnp = reader.GetString(8);
                    DateTime registrationDate = reader.GetDateTime(9);
                    string avatarUrl = reader.GetString(10);
                    string bloodType = reader.GetString(11);
                    string emergencyContact = reader.GetString(12);
                    string allergies = reader.GetString(13);
                    double weight = reader.GetDouble(14);
                    int height = reader.GetInt32(15);
                    string password = reader.GetString(16);
                    PatientJointModel patient = new PatientJointModel(userId, patientId, name, bloodType, emergencyContact, allergies, weight, height, username, password, mail, birthDate, cnp, address, phoneNumber, registrationDate);

                    patientList.Add(patient);
                }
                return patientList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<PatientJointModel>();
            }
        }
        public async Task<PatientJointModel> GetPatientByUserId(int UserId)
        {
            const string querySelectPatients = @"SELECT 
                    p.PatientId,
                    p.UserId,
                    u.Username,
                    u.Name,
                    u.BirthDate,
                    u.Mail,
                    u.PhoneNumber,
                    u.Address,
                    u.Cnp,
                    u.RegistrationDate,
                    u.AvatarUrl,
                    p.BloodType,
                    p.EmergencyContact,
                    p.Allergies,
                    p.Weight,
                    p.Height,
                    u.Password
                FROM Patients p
                INNER JOIN Users u ON p.UserId = u.UserId 
                WHERE u.UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand selectCommand = new SqlCommand(querySelectPatients, connection);
                selectCommand.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int patientId = reader.GetInt32(0);
                    int userId = reader.GetInt32(1);
                    string username = reader.GetString(2);
                    string name = reader.GetString(3);
                    DateTime birthDateTime = reader.GetDateTime(4);
                    DateOnly birthDate = DateOnly.FromDateTime(birthDateTime);
                    string mail = reader.GetString(5);
                    string? phoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6);
                    string? address = reader.IsDBNull(7) ? null : reader.GetString(7);
                    string cnp = reader.GetString(8);
                    DateTime registrationDate = reader.GetDateTime(9);
                    string avatarUrl = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    string bloodType = reader.GetString(11);
                    string emergencyContact = reader.GetString(12);
                    string? allergies = reader.IsDBNull(13) ? null : reader.GetString(13);
                    double weight = reader.GetDouble(14);
                    int height = reader.GetInt32(15);
                    string password = reader.GetString(16);
                    return new PatientJointModel(userId, patientId, name, bloodType, emergencyContact, allergies, weight, height, username, password, mail, birthDate, cnp, address, phoneNumber, registrationDate);
                }
                return new PatientJointModel(0, 0, "", "", "", "", 0, 0, "", "", "", new DateOnly(), "", "", "", new DateTime());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new PatientJointModel(0, 0, "", "", "", "", 0, 0, "", "", "", new DateOnly(), "", "", "", new DateTime());
            }
        }
        public async Task<bool> UpdatePassword(int UserId, string Password)
        {
            const string queryUpdatePassword = @"UPDATE Users SET Password = @Password WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdatePassword, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Password", Password);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateEmail(int UserId, string Email)
        {
            const string queryUpdateEmail = @"UPDATE Users SET Mail = @Email WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateEmail, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Email", Email);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateUsername(int UserId, string Username)
        {
            const string queryUpdateUsername = @"UPDATE Users SET Username = @Username WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateUsername, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Username", Username);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateName(int UserId, string Name)
        {
            const string queryUpdateName = @"UPDATE Users SET Name = @Name WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateName, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Name", Name);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateBirthDate(int UserId, DateOnly BirthDate)
        {
            const string queryUpdateBirthDate = @"UPDATE Users SET BirthDate = @BirthDate WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateBirthDate, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@BirthDate", BirthDate);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateAddress(int UserId, string Address)
        {
            const string queryUpdateAddress = @"UPDATE Users SET Address = @Address WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateAddress, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Address", Address);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdatePhoneNumber(int UserId, string PhoneNumber)
        {
            const string queryUpdatePhoneNumber = @"UPDATE Users SET PhoneNumber = @PhoneNumber WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdatePhoneNumber, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateEmergencyContact(int UserId, string EmergencyContact)
        {
            const string queryUpdateEmergencyContact = @"UPDATE Patients SET EmergencyContact = @EmergencyContact WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateEmergencyContact, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@EmergencyContact", EmergencyContact);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateWeight(int UserId, double Weight)
        {
            const string queryUpdateWeight = @"UPDATE Patients SET Weight = @Weight WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateWeight, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Weight", Weight);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> UpdateHeight(int UserId, int Height)
        {
            const string queryUpdateHeight = @"UPDATE Patients SET Height = @Height WHERE UserId = @UserId;";
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                SqlCommand updateCommand = new SqlCommand(queryUpdateHeight, connection);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@Height", Height);
                await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}