// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DepartmentManagerModel.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the DepartmentManagerModel class for managing hospital departments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Models
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Hospital.Models;
    using Hospital.Services;

    /// <summary>
    /// Model class for managing department data and operations.
    /// </summary>
    public class DepartmentManagerModel
    {
        private readonly ObservableCollection<Department> departmentList;
        private readonly DepartmentsDatabaseService departmentsDBService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentManagerModel"/> class.
        /// </summary>
        /// <param name="departmentsDBService">The department database service.</param>
        public DepartmentManagerModel(DepartmentsDatabaseService departmentsDBService)
        {
            this.departmentsDBService = departmentsDBService;
            this.departmentList = new ObservableCollection<Department>();
        }

        /// <summary>
        /// Loads departments from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadDepartments()
        {
            var departments = await this.departmentsDBService.GetAllDepartments();
            this.departmentList.Clear();
            foreach (var department in departments)
            {
                this.departmentList.Add(department);
            }
        }

        /// <summary>
        /// Gets the list of departments.
        /// </summary>
        /// <returns>An observable collection of departments.</returns>
        public ObservableCollection<Department> GetDepartments()
        {
            return this.departmentList;
        }
    }
}