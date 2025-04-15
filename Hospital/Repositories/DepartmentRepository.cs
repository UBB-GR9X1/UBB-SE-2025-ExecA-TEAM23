using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Configs;
using Hospital.Models;
using Microsoft.Data.SqlClient;
namespace Hospital.Repositories
{
    public class DepartmentRepository
    {
        private readonly Config configs;

        public DepartmentRepository()
        {
            configs = Config.GetInstance();
        }

        public async Task<List<Department>> GetDepartmentsFromDB()
        {
            List<Department> departments = new List<Department>();
            string connectionString = configs.DatabaseConnection;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Departments";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Department department = new Department(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            );
                            //{ ?????????????????????
                            //    Description = reader.GetString(2)
                            //};
                            departments.Add(department);
                        }
                    }
                }
            }
            return departments;
        }
    }
}
