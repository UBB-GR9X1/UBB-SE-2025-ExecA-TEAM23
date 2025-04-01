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