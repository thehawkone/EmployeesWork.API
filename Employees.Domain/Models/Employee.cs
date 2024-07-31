namespace Employees.Domain.Models;

public class Employee
{
    public const int MaxFullNameLength = 150;
    private Employee(Guid id, string fullName, string position, decimal salary)
    {
        Id = id;
        FullName = fullName;
        Position = position;
        Salary = PositionSalaryMap.ContainsKey(position) ? PositionSalaryMap[position] : salary;
    }
    public Guid Id { get; }
    public string FullName { get; } = string.Empty;

    private string _position;
    public string Position
    {
        get => _position;
        set
        {
            if (PositionSalaryMap.ContainsKey(value)) {
                _position = value;
                Salary = PositionSalaryMap[value];
            } else {
                throw new ArgumentException("Unknown position");
            }
        }
    }
    public decimal Salary { get; private set; }

    public static (Employee Employee, string Error) Create(Guid id, string fullName, string position, decimal salary)
    {
        var error = string.Empty;
        
        if (string.IsNullOrEmpty(fullName) || fullName.Length > MaxFullNameLength) {
            error = "Name can not be empty or longer then 150 symbols!";
        }
        
        var employee = new Employee(id, fullName, position, salary);
        
        return (employee, error);
    }
    
    private static readonly Dictionary<string, decimal> PositionSalaryMap = new()
    {
        { "Junior Developer", 3000 },
        { "Middle Developer", 6000 },
        { "Senior Developer", 9000 },
        { "Team Lead", 10000 },
        { "Project Manager", 5000 }
    };
}