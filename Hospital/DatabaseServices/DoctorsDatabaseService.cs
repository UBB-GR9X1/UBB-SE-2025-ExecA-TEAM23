using Hospital.Configs;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Diagnostics;


namespace Hospital.DatabaseServices
{
    public class DoctorsDatabaseService
    {
        private readonly Config _config;

        public DoctorsDatabaseService()
        {
            _config = Config.GetInstance();
        }

        public async Task<DoctorDisplayModel> GetDoctorByUserId(int userId)
        {
            const string query = @"
        SELECT d.DoctorId, u.Name AS DoctorName, 
               d.DepartmentId, dept.DepartmentName, d.DoctorRating,
               d.CareerInfo, u.AvatarUrl, u.PhoneNumber, u.Mail
        FROM Doctors d
        JOIN Users u ON d.UserId = u.UserId
        LEFT JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
        WHERE u.UserId = @userId";

            try
            {
                using var connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync();

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new DoctorDisplayModel(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.IsDBNull(3) ? null : reader.GetString(3),
                        reader.GetDouble(4),
                        reader.IsDBNull(5) ? null : reader.GetString(5),
                        reader.IsDBNull(6) ? null : reader.GetString(6),
                        reader.IsDBNull(7) ? null : reader.GetString(7),
                        reader.IsDBNull(8) ? null : reader.GetString(8)
                    );
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting doctor by user ID: {ex.Message}");
                return null;
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
                    double rating = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4);
                    string careerInfo = reader.IsDBNull(5) ? null : reader.GetString(5);
                    string avatarUrl = reader.IsDBNull(6) ? null : reader.GetString(6);
                    string phoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7);
                    string mail = reader.IsDBNull(8) ? null : reader.GetString(8);

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
                    double rating = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4);
                    string careerInfo = reader.IsDBNull(5) ? null : reader.GetString(5);
                    string avatarUrl = reader.IsDBNull(6) ? null : reader.GetString(6);
                    string phoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7);
                    string mail = reader.IsDBNull(8) ? null : reader.GetString(8);

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

        public async Task<bool> UpdateDoctorName(int doctorId, string newName)
        {
            const string queryUpdateDoctorName = @"
            UPDATE Users
            SET Name = @NewName
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateDoctorName, connection);
                updateCommand.Parameters.AddWithValue("@NewName", newName);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        public async Task<bool> UpdateDoctorDepartment(int doctorId, int newDepartmentId)
        {
            const string queryUpdateDepartment = @"
            UPDATE Doctors
            SET DepartmentId = @NewDepartmentId
            WHERE DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateDepartment, connection);
                updateCommand.Parameters.AddWithValue("@NewDepartmentId", newDepartmentId);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        public async Task<bool> UpdateDoctorRating(int doctorId, double newRating)
        {
            const string queryUpdateRating = @"
            UPDATE Doctors
            SET DoctorRating = @NewRating
            WHERE DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateRating, connection);
                updateCommand.Parameters.AddWithValue("@NewRating", newRating);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        public async Task<bool> UpdateDoctorCareerInfo(int doctorId, string newCareerInfo)
        {
            const string queryUpdateCareerInfo = @"
            UPDATE Doctors
            SET CareerInfo = @NewCareerInfo
            WHERE DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateCareerInfo, connection);
                updateCommand.Parameters.AddWithValue("@NewCareerInfo", (object)newCareerInfo ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        public async Task<bool> UpdateDoctorAvatarUrl(int doctorId, string newAvatarUrl)
        {
            const string queryUpdateAvatarUrl = @"
            UPDATE Users
            SET AvatarUrl = @NewAvatarUrl
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateAvatarUrl, connection);
                updateCommand.Parameters.AddWithValue("@NewAvatarUrl", (object)newAvatarUrl ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        public async Task<bool> UpdateDoctorPhoneNumber(int doctorId, string newPhoneNumber)
        {
            const string queryUpdatePhoneNumber = @"
            UPDATE Users
            SET PhoneNumber = @NewPhoneNumber
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdatePhoneNumber, connection);
                updateCommand.Parameters.AddWithValue("@NewPhoneNumber", (object)newPhoneNumber ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        public async Task<int> GetDoctorUserId(int doctorId)
        {
            const string query = "SELECT UserId FROM Doctors WHERE DoctorId = @doctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@doctorId", doctorId);

                var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting doctor user ID: {ex.Message}");
                return -1;
            }
        }

        public async Task<bool> UpdateDoctorEmail(int doctorId, string newEmail)
        {
            const string queryUpdateEmail = @"
            UPDATE Users
            SET Mail = @NewEmail
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateEmail, connection);
                updateCommand.Parameters.AddWithValue("@NewEmail", (object)newEmail ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

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

        // A comprehensive method to update multiple doctor properties at once
        public async Task<bool> UpdateDoctorInfo(DoctorDisplayModel doctor)
        {
            // We'll need two separate queries since the data is split across two tables
            const string queryUpdateDoctorTable = @"
            UPDATE Doctors
            SET DepartmentId = @DepartmentId,
                DoctorRating = @Rating,
                CareerInfo = @CareerInfo
            WHERE DoctorId = @DoctorId";

            const string queryUpdateUserTable = @"
            UPDATE Users
            SET Name = @DoctorName,
                AvatarUrl = @AvatarUrl,
                PhoneNumber = @PhoneNumber,
                Mail = @Mail
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);
                
                // Start a transaction to ensure both updates succeed or fail together
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Update Doctors table
                    SqlCommand updateDoctorCommand = new SqlCommand(queryUpdateDoctorTable, connection, transaction);
                    updateDoctorCommand.Parameters.AddWithValue("@DepartmentId", doctor.DepartmentId);
                    updateDoctorCommand.Parameters.AddWithValue("@Rating", (object)doctor.Rating ?? DBNull.Value);
                    updateDoctorCommand.Parameters.AddWithValue("@CareerInfo", (object)doctor.CareerInfo ?? DBNull.Value);
                    updateDoctorCommand.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);

                    await updateDoctorCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                    // Update Users table
                    SqlCommand updateUserCommand = new SqlCommand(queryUpdateUserTable, connection, transaction);
                    updateUserCommand.Parameters.AddWithValue("@DoctorName", doctor.DoctorName);
                    updateUserCommand.Parameters.AddWithValue("@AvatarUrl", (object)doctor.AvatarUrl ?? DBNull.Value);
                    updateUserCommand.Parameters.AddWithValue("@PhoneNumber", (object)doctor.PhoneNumber ?? DBNull.Value);
                    updateUserCommand.Parameters.AddWithValue("@Mail", (object)doctor.Mail ?? DBNull.Value);
                    updateUserCommand.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);

                    await updateUserCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                    // Commit the transaction
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    // Roll back the transaction if something fails
                    transaction.Rollback();
                    Console.WriteLine(e.Message);
                    return false;
                }
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
    }
}
