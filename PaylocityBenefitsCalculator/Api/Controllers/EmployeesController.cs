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
    [HttpGet("paycheck/{employee_id}")]
    public async Task<ActionResult<ApiResponse<BenefitResponse>>> GetPaycheck(int employee_id) {
        var result = new ApiResponse<BenefitResponse>{
            Data = null,
            Success = true
        };
        try {
            // could have this object already if send from backend 
            var dbEmployee = await _companyRepository.GetEmployeeById(employee_id);
            if(dbEmployee != null) {
                var paycheck = _benefitsCalculatorService.createBenefitsPackage(dbEmployee);
                result.Data = paycheck;
                return Ok(result);
            } else {
                result.Data = null;
                result.Success = true;
                result.Message = $"Can not find employee with id = {employee_id}";
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
