using System.Text.Json;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        string empText = await System.IO.File.ReadAllTextAsync(@"Dtos/EmployeeData.json");
        var response = JsonSerializer.Deserialize<List<GetEmployeeDto>>(empText);
        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = null,
            Success = true
        };
         if(response != null) {
            GetEmployeeDto employee = response.Find(x => x.Id == id);
            result.Data = employee;
            if(result.Data == null) {
                result.Message = $"Can not find employee with id = {id}";
            }
        }
        else {
            result.Success = false;
        }

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {

        string empText = await System.IO.File.ReadAllTextAsync(@"Dtos/EmployeeData.json");
        var response = JsonSerializer.Deserialize<List<GetEmployeeDto>>(empText);
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = null,
            Success = true
        };

        if (response != null)
        {
            result.Data = response;
        } else {
            result.Success = false;
        }

        return result;
    }
}
