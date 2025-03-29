using Hospital.Configs;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace Hospital.DatabaseServices
{
    public class DoctorsDatabaseService
    {
        private readonly Config _config;

        public DoctorsDatabaseService()
        {
            _config = Config.GetInstance();
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

    }
}
