using Hospital.Configs;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using System.Linq;

namespace Hospital.DatabaseServices
{
    public class DoctorsDatabaseService
    {
        private readonly Config _config;

        public DoctorsDatabaseService()
        {
            _config = Config.GetInstance();
        }
        
        public async Task<DoctorDisplayModel> GetDoctorById(int userId)
        {
            const string query = @"SELECT 
                d.DoctorId,
                u.Name AS DoctorName,
                d.DepartmentId,
                dep.DepartmentName,
                d.DoctorRating,
                d.CareerInfo,
                u.AvatarUrl, 
                u.PhoneNumber,
                u.Mail
            FROM Doctors d
            INNER JOIN Users u ON d.UserId = u.UserId
            LEFT JOIN Departments dep ON d.DepartmentId = dep.DepartmentId
            WHERE d.UserId = @userId";
            
            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

                using SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                
                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    Console.WriteLine($"Doctor found with user ID: {userId}");

                    return new DoctorDisplayModel(
                        doctorId: reader.GetInt32(0),
                        doctorName: reader.GetString(1),
                        departmentId: reader.GetInt32(2),
                        departmentName: reader.GetString(3),
                        rating: reader.GetDouble(4),
                        careerInfo: reader.IsDBNull(5) ? "" : reader.GetString(5),
                        avatarUrl: reader.IsDBNull(6) ? "" : reader.GetString(6),
                        phoneNumber: reader.IsDBNull(7) ? "" : reader.GetString(7),
                        mail: reader.GetString(8)
                    );
                }

                throw new Exception($"No doctor found with user ID: {userId}");
            }
            catch (SqlException e)
            {
                throw new Exception($"SQL Error: {e.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e.Message}");
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

        public async Task<List<DoctorDisplayModel>> GetDoctorsByDepartmentPartialName(string departmentPartialName)
        {
            const string querySelectDoctors = @"
            SELECT 
                d.DoctorId,
                u.Name AS DoctorName,
                d.DepartmentId,
                dept.DepartmentName,
                d.DoctorRating AS Rating,
                d.CareerInfo,
                u.AvatarUrl,
                u.PhoneNumber,
                u.Mail
            FROM Doctors d
            INNER JOIN Users u ON d.UserId = u.UserId
            INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
            WHERE dept.DepartmentName LIKE '%' + @departmentNamePartial + '%'";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(querySelectDoctors, connection);
                selectCommand.Parameters.AddWithValue("@departmentNamePartial", departmentPartialName);

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                List<DoctorDisplayModel> doctorList = new List<DoctorDisplayModel>();
                if(departmentPartialName == "")
                {
                    return doctorList;
                }
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int doctorId = reader.GetInt32(0);
                    string doctorName = reader.GetString(1);
                    int departmentId = reader.GetInt32(2);
                    string departmentName = reader.GetString(3);
                    double rating = reader.GetDouble(4);
                    string? careerInfo = reader.IsDBNull(5) ? null : reader.GetString(5);
                    string? avatarUrl = reader.IsDBNull(6) ? null : reader.GetString(6);
                    string? phoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7);
                    string? mail = reader.IsDBNull(8) ? null : reader.GetString(8);

                    DoctorDisplayModel doctor = new DoctorDisplayModel(
                        doctorId, doctorName,
                        departmentId, departmentName,
                        rating, careerInfo,
                        avatarUrl, phoneNumber, mail);

                    doctorList.Add(doctor);
                }
                return doctorList;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return new List<DoctorDisplayModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<DoctorDisplayModel>();
            }
        }

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

        public async Task<List<DoctorDisplayModel>> GetDoctorsByPartialDoctorName(string doctorPartialName)
        {
            const string querySelectDoctors = @"
            SELECT 
                d.DoctorId,
                u.Name AS DoctorName,
                d.DepartmentId,
                dept.DepartmentName,
                d.DoctorRating AS Rating,
                d.CareerInfo,
                u.AvatarUrl,
                u.PhoneNumber,
                u.Mail
            FROM Doctors d
            INNER JOIN Users u ON d.UserId = u.UserId
            INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
            WHERE u.Name LIKE '%' + @doctorNamePartial + '%'";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand selectCommand = new SqlCommand(querySelectDoctors, connection);
                selectCommand.Parameters.AddWithValue("@doctorNamePartial", doctorPartialName);

                SqlDataReader reader = await selectCommand.ExecuteReaderAsync().ConfigureAwait(false);
                List<DoctorDisplayModel> doctorList = new List<DoctorDisplayModel>();

                if (doctorPartialName == "")
                {
                    return doctorList;
                }

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    int doctorId = reader.GetInt32(0);
                    string doctorName = reader.GetString(1);
                    int departmentId = reader.GetInt32(2);
                    string departmentName = reader.GetString(3);
                    double rating = reader.GetDouble(4);
                    string careerInfo = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    string avatarUrl = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    string phoneNumber = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    string mail = reader.GetString(8);

                    DoctorDisplayModel doctor = new DoctorDisplayModel(
                        doctorId, doctorName,
                        departmentId, departmentName,
                        rating, careerInfo,
                        avatarUrl, phoneNumber, mail);

                    doctorList.Add(doctor);
                }
                return doctorList;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return new List<DoctorDisplayModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<DoctorDisplayModel>();
            }
        }

        public async Task<bool> UpdateDoctorName(int userId, string newName)
        {
            const string queryUpdateDoctorName = @"
            UPDATE Users
            SET Name = @newName
            WHERE UserId = @userId;";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateDoctorName, connection);
                updateCommand.Parameters.AddWithValue("@newName", newName);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateDoctorDepartment(int userId, int newDepartmentId)
        {
            if (newDepartmentId <= 0)
            {
                Console.WriteLine("Invalid department ID");
                return false;
            }

            const string queryUpdateDepartment = @"
            UPDATE Doctors
            SET DepartmentId = @newDepartmentId
            WHERE UserId = @userId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateDepartment, connection);
                updateCommand.Parameters.AddWithValue("@newDepartmentId", newDepartmentId);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateDoctorRating(int userId, double newRating)
        {
            const string queryUpdateRating = @"
            UPDATE Doctors
            SET DoctorRating = @newRating
            WHERE UserId = @userId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateRating, connection);
                updateCommand.Parameters.AddWithValue("@newRating", newRating);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateDoctorCareerInfo(int userId, string newCareerInfo)
        { 
            const string queryUpdateCareerInfo = @"
            UPDATE Doctors
            SET CareerInfo = @newCareerInfo
            WHERE UserId = @userId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateCareerInfo, connection);
                updateCommand.Parameters.AddWithValue("@newCareerInfo", newCareerInfo);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateDoctorAvatarUrl(int userId, string newAvatarUrl)
        {
            const string queryUpdateAvatarUrl = @"
            UPDATE Users
            SET AvatarUrl = @newAvatarUrl
            WHERE UserId = @userId;";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateAvatarUrl, connection);
                updateCommand.Parameters.AddWithValue("@newAvatarUrl", newAvatarUrl);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateDoctorPhoneNumber(int userId, string newPhoneNumber)
        {
            const string queryUpdatePhoneNumber = @"
            UPDATE Users
            SET PhoneNumber = @newPhoneNumber
            WHERE UserId = @userId;";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdatePhoneNumber, connection);
                updateCommand.Parameters.AddWithValue("@newPhoneNumber", (object)newPhoneNumber ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateDoctorEmail(int userId, string newEmail)
        {
            const string queryUpdateEmail = @"
            UPDATE Users
            SET Mail = @newEmail
            WHERE UserId = @userId;";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateEmail, connection);
                updateCommand.Parameters.AddWithValue("@newEmail", newEmail);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}