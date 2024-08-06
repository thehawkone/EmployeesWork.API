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