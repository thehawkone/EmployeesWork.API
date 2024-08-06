namespace Employees.Application.Services;

public record EmployeesRequest(
    string FullName,
    string Position
    );