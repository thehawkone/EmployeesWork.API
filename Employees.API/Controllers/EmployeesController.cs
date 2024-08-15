using System.Security.Cryptography;
using Employees.API.Contracts;
using Employees.Application.Services;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employees.API.Controllers;
[ApiController]
[Route("[action]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesService _employeesService;
    
    public EmployeesController(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeesResponse>>> GetEmployees()
    {
        var employees = await _employeesService.GetAllEmployees();

        var response= employees
            .Select(e => new EmployeesResponse(e.Id, e.FullName, e.Position, e.Salary));

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeesResponse>>> GetEmployeesWithPagination(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "name",
        [FromQuery] string sortOrder = "asc",
        [FromQuery] string name = null,
        [FromQuery] string position = null)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var employees = await _employeesService.GetAllEmployees();

        if (!string.IsNullOrEmpty(name)) {
            employees = employees.Where(e => e.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrEmpty(position)) {
            employees = employees.Where(e => e.Position.Contains(position, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        switch (sortBy.ToLower()) {
            case "fullname":
                employees = sortOrder.ToLower() == "desc"
                    ? employees.OrderByDescending(e => e.FullName).ToList()
                    : employees.OrderBy(e => e.FullName).ToList();
                break;
            case "position":
                employees = sortOrder.ToLower() == "desc"
                    ? employees.OrderByDescending(e => e.Position).ToList()
                    : employees.OrderBy(e => e.Position).ToList();
                break;
            case "salary":
                employees = sortOrder.ToLower() == "desc"
                    ? employees.OrderByDescending(e => e.Salary).ToList()
                    : employees.OrderBy(e => e.Salary).ToList();
                break;
            default: 
                employees = sortOrder.ToLower() == "desc"
                    ? employees.OrderByDescending(e => e.FullName).ToList()
                    : employees.OrderBy(e => e.FullName).ToList();
                break;
        }

        var totalItems = employees.Count;
        var pagedEmployees = employees
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    var response = new PagedResponse<EmployeesResponse>(
        pageNumber,
        pageSize,
        totalItems,
        (int)Math.Ceiling(totalItems / (double)pageSize),
        pagedEmployees.Select(e => new EmployeesResponse(e.Id, e.FullName, e.Position, e.Salary)).ToList()
    );

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateEmployee([FromBody] EmployeesRequest request)
    {
        var (employee, error) = Employee.Create(
            Guid.NewGuid(),
            request.FullName,
            request.Position
        );

        if (!string.IsNullOrEmpty(error)) {
            return BadRequest(error);
        }

        var employeeId = await _employeesService.CreateEmployee(employee);

        return Ok(employeeId);
    }
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateEmployee(Guid id, [FromBody] EmployeesRequest request)
    {
        var employeeId = await _employeesService.UpdateEmployee(id, request.FullName, request.Position);

        return Ok(employeeId);
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteEmployee(Guid id)
    {
        return Ok(await _employeesService.DeleteEmployee(id));
    }
}