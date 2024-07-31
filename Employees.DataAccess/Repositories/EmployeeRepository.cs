using Employees.DataAccess.Entites;
using Employees.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeesDbContext _context;
    
    public EmployeeRepository(EmployeesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> Get()
    {
        var employeeEntities = await _context.Employees
            .AsNoTracking()
            .ToListAsync();

        var employees = employeeEntities
            .Select(e => Employee.Create(e.Id, e.FullName, e.Position, e.Salary).Employee)
            .ToList();

        return employees;
    }

    public async Task<Guid> Create(Employee employee)
    {
        var employeeEntity = new EmployeeEntity
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Position = employee.Position,
            Salary = employee.Salary
        };

        await _context.Employees.AddAsync(employeeEntity);
        await _context.SaveChangesAsync();
        
        return employee.Id;
    }

    public async Task<Guid> Update(Guid id, string fullName, string position)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) {
            throw new InvalidOperationException("Employee not found!");
        }
        
        //Создаем новый экземпляр сотрудника с обновленными данными
        var (updatedEmployee, error) = Employee.Create(id, fullName, position, employee.Salary);

        if (string.IsNullOrEmpty(error)) {
            throw new InvalidOperationException(error);
        }

        await _context.Employees
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(k => k
                .SetProperty(e => e.FullName, updatedEmployee.FullName)
                .SetProperty(e => e.Position, updatedEmployee.Position)
                .SetProperty(e => e.Salary, updatedEmployee.Salary));

        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Employees
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}