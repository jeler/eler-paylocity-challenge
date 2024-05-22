using Api.Dtos.Dependent;
using Api.Dtos.Employee;

public interface ICompanyRepository
{
    public Task<List<GetEmployeeDto>> GetEmployees();
    public Task<GetEmployeeDto> GetEmployeeById(int id);
    public Task<List<GetDependentDto>> GetDependents();
    public Task<GetDependentDto> GetDependentById(int id);

}