using Employees.DataAccess.Entites;
using Employees.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName)
            .HasMaxLength(Employee.MaxFullNameLength)
            .IsRequired();

        builder.Property(e => e.Position)
            .IsRequired();

        builder.Property(e => e.Salary)
            .IsRequired();
    }
}