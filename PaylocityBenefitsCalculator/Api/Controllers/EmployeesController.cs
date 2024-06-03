using System.Text.Json;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Apis.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    readonly ICompanyRepository _companyRepository;

    // private IBenefitsCalculatorService _benefitsCalculator;
    public IBenefitsCalculatorService _benefitsCalculatorService;
    public EmployeesController(ICompanyRepository companyRepository, IBenefitsCalculatorService benefitsCalculator)
    {
        _companyRepository = companyRepository;
        _benefitsCalculatorService = benefitsCalculator;
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
                return NotFound(result);
            }
            return Ok(result);
        } catch(Exception e) {
        
            result.Success = false;
            // would sanitize this message so that database information would not be exposed
            result.Error = e.Message;
            return BadRequest(result);
        }
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
            return Ok(result);
        } catch(Exception e) {
            result.Success = false;
            // will return SQL so want mapping here to an appropriate user response
            result.Message= e.Message;
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    // Returns employee paycheck 
    // makes it seem like a paycheck id and not an employee id 
    [SwaggerOperation(Summary = "Get Paycheck")]
    [HttpGet("paycheck/{id}")]
    public async Task<ActionResult<ApiResponse<IBenefitsCalculator>>> GetPaycheck(int id) {
        var result = new ApiResponse<IBenefitsCalculator>{
            Data = null,
            Success = true
        };
        try {
            // could have this object already if send from backend 
            var dbEmployee = await _companyRepository.GetEmployeeById(id);
            if(dbEmployee != null) {
                // Not sure how much of this info we want to share with user
                // injected the benefits calculator into controller and would return methods 
                // Would return paycheck object
                // consumer of calculator would not have access to variables
                // var paycheck = new BenefitsCalculator(dbEmployee);
                var paycheck = _benefitsCalculatorService.generatePayload(dbEmployee);
                result.Data = paycheck;
                return Ok(result);
            } else {
                result.Data = null;
                result.Success = true;
                result.Message = $"Can not find employee with id = {id}";
                return NotFound(result);

            }
        } catch(Exception e) {
            result.Success = false;
            // would sanitize this message so that database information would not be exposed
            result.Error = e.Message;
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
