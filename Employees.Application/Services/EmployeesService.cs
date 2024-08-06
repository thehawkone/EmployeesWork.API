using Employees.DataAccess.Repositories;
using Employees.Domain.Models;

namespace Employees.Application.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public EmployeesService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        return await _employeeRepository.Get();
    }

    public async Task<Guid> CreateEmployee(Employee employee)
    {
        return await _employeeRepository.Create(employee);
    }

    public async Task<Guid> UpdateEmployee(Guid id, string fullName, string position)
    {
        return await _employeeRepository.Update(id, fullName, position);
    }

    public async Task<Guid> DeleteEmployee(Guid id)
    {
        return await _employeeRepository.Delete(id);
    }
}