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
        throw new NotImplementedException();
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
