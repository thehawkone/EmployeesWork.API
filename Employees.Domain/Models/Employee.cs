namespace Employees.Domain.Models;

public class Employee
{
    public const int MaxFullNameLength = 150;
    public Guid Id { get; }
    public string FullName { get; } = string.Empty;
    
    public string Position { get; }
    public decimal Salary { get; private set; }
    private Employee(Guid id, string fullName, string position, decimal salary)
    {
        Id = id;
        FullName = fullName;
        Position = position;
        Salary = salary;
    }

    public static (Employee Employee, string Error) Create(Guid id, string fullName, string position)
    {
        var salary = CalculateSalary(position);
        var error = string.Empty;
        
        if (string.IsNullOrEmpty(fullName) || fullName.Length > MaxFullNameLength) {
            error = "Name can not be empty or longer then 150 symbols!";
        }
        
        var employee = new Employee(id, fullName, position, salary);
        return (employee, error);
    }

    private static decimal CalculateSalary(string position)
    {
        return position switch
        {
            "Junior" => 3000,
            "Middle" => 6000,
            "Senior" => 9000
        };
    }
}