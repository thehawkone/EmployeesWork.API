using Employees.Domain.Models;

namespace Employees.DataAccess.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> Get();
    Task<Guid> Create(Employee employee);
    Task<Guid> Update(Guid id, string fullName, string position);
    Task<Guid> Delete(Guid id);
}