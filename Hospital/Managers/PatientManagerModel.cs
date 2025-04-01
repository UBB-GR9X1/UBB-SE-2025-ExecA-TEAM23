using Hospital.Configs;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Hospital.DatabaseServices
{
    public class DoctorManagerModel
    {
        private readonly Config _config;

        public DoctorManagerModel()
        {
            _config = Config.GetInstance();
        }

        public DoctorManagerModel()
        {
            _config = Config.GetInstance();
        }

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
        
        public async Task<bool> UpdateDoctorInfo(DoctorJointModel doctor)
        {
            // We'll need two separate queries since the data is split across two tables
            const string queryUpdateDoctorTable = @"
            UPDATE Doctors
            SET DepartmentId = @DepartmentId,
                DoctorRating = @DoctorRating,
                LicenseNumber = @LicenseNumber
            WHERE DoctorId = @DoctorId";

            const string queryUpdateUserTable = @"
            UPDATE Users
            SET Username = @Username,
                Password = @Password,
                Mail = @Mail,
                BirthDate = @BirthDate,
                Cnp = @Cnp,
                Address = @Address,
                PhoneNumber = @PhoneNumber
            WHERE UserId = @UserId";

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
                    updateDoctorCommand.Parameters.AddWithValue("@DoctorRating", doctor.DoctorRating);
                    updateDoctorCommand.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);
                    updateDoctorCommand.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);

                    await updateDoctorCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                    // Update Users table
                    SqlCommand updateUserCommand = new SqlCommand(queryUpdateUserTable, connection, transaction);
                    updateUserCommand.Parameters.AddWithValue("@Username", doctor.Username);
                    updateUserCommand.Parameters.AddWithValue("@Password", doctor.Password);
                    updateUserCommand.Parameters.AddWithValue("@Mail", doctor.Mail);
                    updateUserCommand.Parameters.AddWithValue("@BirthDate", doctor.BirthDate);
                    updateUserCommand.Parameters.AddWithValue("@Cnp", doctor.Cnp);
                    updateUserCommand.Parameters.AddWithValue("@Address", doctor.Address);
                    updateUserCommand.Parameters.AddWithValue("@PhoneNumber", doctor.PhoneNumber);
                    updateUserCommand.Parameters.AddWithValue("@UserId", doctor.UserId);

                    await updateUserCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

                    // Commit the transaction
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    // Roll back the transaction if something fails
                    transaction.Rollback();
                    Console.WriteLine($"Transaction Exception: {e.Message}");
                    return false;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
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
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
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
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDoctorLicenseNumber(int doctorId, string newLicenseNumber)
        {
            const string queryUpdateLicense = @"
            UPDATE Doctors
            SET LicenseNumber = @NewLicenseNumber
            WHERE DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateLicense, connection);
                updateCommand.Parameters.AddWithValue("@NewLicenseNumber", newLicenseNumber);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDoctorUsername(int doctorId, string newUsername)
        {
            const string queryUpdateUsername = @"
            UPDATE Users
            SET Username = @NewUsername
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateUsername, connection);
                updateCommand.Parameters.AddWithValue("@NewUsername", newUsername);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDoctorPassword(int doctorId, string newPassword)
        {
            const string queryUpdatePassword = @"
            UPDATE Users
            SET Password = @NewPassword
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdatePassword, connection);
                updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
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
                updateCommand.Parameters.AddWithValue("@NewEmail", newEmail);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDoctorBirthDate(int doctorId, DateOnly newBirthDate)
        {
            const string queryUpdateBirthDate = @"
            UPDATE Users
            SET BirthDate = @NewBirthDate
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateBirthDate, connection);
                updateCommand.Parameters.AddWithValue("@NewBirthDate", newBirthDate);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDoctorCnp(int doctorId, string newCnp)
        {
            const string queryUpdateCnp = @"
            UPDATE Users
            SET Cnp = @NewCnp
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateCnp, connection);
                updateCommand.Parameters.AddWithValue("@NewCnp", newCnp);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDoctorAddress(int doctorId, string newAddress)
        {
            const string queryUpdateAddress = @"
            UPDATE Users
            SET Address = @NewAddress
            FROM Users u
            INNER JOIN Doctors d ON u.UserId = d.UserId
            WHERE d.DoctorId = @DoctorId";

            try
            {
                using SqlConnection connection = new SqlConnection(_config.DatabaseConnection);
                await connection.OpenAsync().ConfigureAwait(false);

                SqlCommand updateCommand = new SqlCommand(queryUpdateAddress, connection);
                updateCommand.Parameters.AddWithValue("@NewAddress", newAddress);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
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
                updateCommand.Parameters.AddWithValue("@NewPhoneNumber", newPhoneNumber);
                updateCommand.Parameters.AddWithValue("@DoctorId", doctorId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rowsAffected > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Exception: {e.Message}");
                return false;
            }
        }
    }
}