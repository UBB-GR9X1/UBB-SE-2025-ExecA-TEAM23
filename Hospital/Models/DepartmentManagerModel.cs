using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hospital.Models;
using Hospital.Services;

public class DepartmentManagerModel
{
    private ObservableCollection<Department> _s_departmentList;
    private DepartmentsDatabaseService _departmentsDBService;

    public DepartmentManagerModel(DepartmentsDatabaseService departmentsDBService)
    {
        _departmentsDBService = departmentsDBService;
        _s_departmentList = new ObservableCollection<Department>();

    }
    public async Task LoadDepartments()
    {
        var departments = await _departmentsDBService.GetDepartmentsFromDB();
        _s_departmentList.Clear();
        foreach(var department in departments)
        {
            _s_departmentList.Add(department);
        }
    }
    public ObservableCollection<Department> GetDepartments()
    {
        return _s_departmentList;
    }
}