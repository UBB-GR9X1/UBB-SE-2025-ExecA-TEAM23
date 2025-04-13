using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Configs;
using Hospital.Interfaces;
using Hospital.Models;
using Microsoft.Data.SqlClient;
namespace Hospital.Services
{
    public class DepartmentsDatabaseService : IDepartmentService
    {
        private readonly IConfigProvider _configProvider;

        public DepartmentsDatabaseService(IConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                using SqlConnection connection = new SqlConnection(_configProvider.GetDatabaseConnection());
                await connection.OpenAsync().ConfigureAwait(false);

                const string query = "SELECT DepartmentId, DepartmentName FROM Departments";

                using SqlCommand command = new SqlCommand(query, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    Department department = new Department(
                        reader.GetInt32(0),
                        reader.GetString(1)
                    );
                    departments.Add(department);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
            }

            return departments;
        }
    }
}
