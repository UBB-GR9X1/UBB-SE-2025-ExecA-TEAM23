using Hospital.Models;
using Hospital.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public class DepartmentService
{
    private ObservableCollection<Department> s_departmentList;
    private DepartmentRepository _departmentRepository;

    public DepartmentService(DepartmentRepository departmentsDBService)
    {
        _departmentRepository = departmentsDBService;
        s_departmentList = new ObservableCollection<Department>();

    }
    public async Task LoadDepartments()
    {
        var departments = await _departmentRepository.GetDepartmentsFromDB();
        s_departmentList.Clear();
        foreach (var department in departments)
        {
            s_departmentList.Add(department);
        }
    }
    public ObservableCollection<Department> GetDepartments()
    {
        return s_departmentList;
    }
}