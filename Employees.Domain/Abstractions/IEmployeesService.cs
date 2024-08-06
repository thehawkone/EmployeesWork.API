using Employees.Domain.Models;

namespace Employees.Application.Services;

public interface IEmployeesService
{
    Task<List<Employee>> GetAllEmployees();
    Task<Guid> CreateEmployee(Employee employee);
    Task<Guid> UpdateEmployee(Guid id, string fullName, string position);
    Task<Guid> DeleteEmployee(Guid id);
}