using System.Text.Json;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    readonly ICompanyRepository _companyRepository;
    public DependentsController(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var result = new ApiResponse<GetDependentDto>
        {
            Success = true
        };
        try {
            // when dependent = null, returning bad request instead of NotFound()
            // Most likely has to do with FirstAsync method used instead of Find()
            var dependent = await _companyRepository.GetDependentById(id);
            if(dependent != null) {
                result.Data = dependent;
            } 
            return Ok(result);
        }
        catch (Exception ex) {
            result.Success = false;
            result.Data = null;
            // would log this and not return anything specific to user
            // result.Message = ex.Message;
            return NotFound(result);
        }

    }


    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = null,
            Success = true
        };
        try {
            var response = await _companyRepository.GetDependents();
            if(response != null) {
                result.Data = response;
            }
            return Ok(result);
        } catch(Exception e) {
            result.Success = false;
            // result.Message = e.Message;
            return BadRequest(result);
        }
    }
}
