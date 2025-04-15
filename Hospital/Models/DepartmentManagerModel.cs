using Hospital.Models;
using Hospital.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public class DepartmentManagerModel
{
    private ObservableCollection<Department> s_departmentList;
    private DepartmentsDatabaseService _departmentsDBService;

    public DepartmentManagerModel(DepartmentsDatabaseService departmentsDBService)
    {
        _departmentsDBService = departmentsDBService;
        s_departmentList = new ObservableCollection<Department>();

    }
    public async Task LoadDepartments()
    {
        var departments = await _departmentsDBService.GetDepartmentsFromDB();
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