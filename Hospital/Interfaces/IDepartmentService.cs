// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDepartmentService.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the IDepartmentService interface for managing hospital departments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hospital.Models;

    /// <summary>
    /// Interface for department-related operations and data retrieval.
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// Gets a list of all departments in the hospital.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of departments.</returns>
        Task<List<Department>> GetAllDepartments();
    }
}