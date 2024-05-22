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
        var response = await _companyRepository.GetDependentById(id);
        var result = new ApiResponse<GetDependentDto>
        {
            Data = null,
            Success = true
        };
        if(response != null) {
            result.Data = response;
            if(result.Data == null) {
                result.Message = $"Can not find dependent with id = {id}";
            }
        }
        else {
            result.Success = false;
        }

        return result;

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
        } catch(Exception e) {
            result.Success = false;
            result.Message = e.Message;
        }
        return result;
    }
}
