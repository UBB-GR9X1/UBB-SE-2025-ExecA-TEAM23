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
    }

}