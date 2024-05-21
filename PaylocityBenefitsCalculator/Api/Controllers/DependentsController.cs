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
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        // type conflict with API Response
        string text = await System.IO.File.ReadAllTextAsync(@"Dtos/Dependent/DependentData.json");
        var response = JsonSerializer.Deserialize<List<GetDependentDto>>(text);
        var result = new ApiResponse<GetDependentDto>
        {
            Data = null,
            Success = true
        };
        if(response != null) {
            // GetDependentDto dependent = response.Where(x => x.Id == id);
            GetDependentDto dependent = response.Find(x => x.Id == id);
            result.Data = dependent;
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
        string text = await System.IO.File.ReadAllTextAsync(@"Dtos/Dependent/DependentData.json");
        var response = JsonSerializer.Deserialize<List<GetDependentDto>>(text);
        var result = new ApiResponse<List<GetDependentDto>>
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
