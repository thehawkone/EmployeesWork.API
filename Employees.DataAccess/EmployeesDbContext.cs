using Employees.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess;

public class EmployeesDbContext : DbContext
{
    public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<EmployeeEntity> Employees { get; set; }
}