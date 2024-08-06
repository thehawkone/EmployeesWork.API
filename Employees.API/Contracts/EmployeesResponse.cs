namespace Employees.API.Contracts;

public record EmployeesResponse(
    Guid Id,
    string FullName,
    string Position,
    decimal Salary
    );