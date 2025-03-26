using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Department
    {
        private int DepartmentId { get; set; }
        private string Name { get; set; }

        public Department(int departmentId, string name)
        {
            this.DepartmentId = departmentId;
            this.Name = name;
        }
    }
}