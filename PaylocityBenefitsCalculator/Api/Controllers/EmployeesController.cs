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
    readonly ICompanyRepository _companyRepository;
    public EmployeesController(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = null,
            Success = true
        };
        try {
            var employee = await _companyRepository.GetEmployeeById(id);
            if(employee != null) {
                result.Data = employee;
            } else {
                result.Message = $"Can not find employee with id = {id}";
            }
        } catch(Exception e) {
            result.Success = false;
            // would sanitize this message so that database information would not be exposed
            result.Error = e.Message;
        }

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
                Data = null,
                Success = true
        };
        try {
            var employees = await _companyRepository.GetEmployees();
            result.Data = employees;
            result.Success = true;
        } catch(Exception e) {
            result.Success = false;
            result.Message= e.Message;
        }

        return result;
    }
    // Returns employee paycheck 
    [SwaggerOperation(Summary = "Get Paycheck")]
    [HttpGet("paycheck/{id}")]
    public async Task<ActionResult<ApiResponse<Benefit>>> GetPaycheck(int id) {
        var result = new ApiResponse<Benefit>{
            Data = null,
            Success = true
        };
        try {
            var dbEmployee = await _companyRepository.GetEmployeeById(id);
            var paycheck = new Benefit(dbEmployee);
            result.Data = paycheck;
            result.Success = true;
        } catch(Exception e) {
            result.Success = false;
            // would sanitize this message so that database information would not be exposed
            result.Error = e.Message;
        }

        return result;
    }
}
